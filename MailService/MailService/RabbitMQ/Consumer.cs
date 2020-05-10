using MailService.Controllers;
using MailService.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MailService.Application.EmailSender;

namespace MailService.RabbitMQ
{
    public class Consumer : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private string queueName;
        private string exch;

        public Consumer(string exch)
        {
            this.exch = exch;
            InitRabbitMQ(exch);

        }
        private void InitRabbitMQ(string exch)
        {
            var factory = new ConnectionFactory() { HostName = "34.87.59.19" }; ;

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exch, type: ExchangeType.Fanout);

            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: exch,
                              routingKey: "");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var email = new EmailSender();
                switch (this.exch)
                {
                    case "AccountToEmail":
                        var messageAccountDTO = JsonConvert.DeserializeObject<MessageAccountDTO>(message);
                        email.SendEmailAsync(messageAccountDTO.Email, "Welcome To Contento System", "Username: " + messageAccountDTO.FullName + ", Password: " + messageAccountDTO.Password);
                        break;
                    case "CreateCampaign":
                        var campaignDTO = JsonConvert.DeserializeObject<CampaignData>(message);
                        email.SendEmailAsync(campaignDTO.EmailEditor, "Welcome To Contento System", "You Have Campaign new: " + campaignDTO.Title);
                        break;
                    case "CreateTask":
                        var TaskDTO = JsonConvert.DeserializeObject<TasksViewModelMessage>(message);
                        email.SendEmailAsync(TaskDTO.EmailWriter, "Welcome To Contento System", "You Have new task: " + TaskDTO.Title);
                        break;
                    case "DedlineTask":
                        var TaskDeadlineDTO = JsonConvert.DeserializeObject<List<CheckDeadlineModel>>(message);
                        foreach (var item in TaskDeadlineDTO)
                        {
                            var lstTitle = ""; 
                            foreach (var item1 in item.ListTask)
                            {
                                lstTitle += item1 + ",";
                            }
                            email.SendEmailAsync(item.Email, "Welcome To Contento System", "You Have task Deadline in tomorrow: " + lstTitle);
                        }
                      
                        break;
                }

            };
            //consumer.Received += async (model, ea) =>
            //{
            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);
            //    var messageAccountDTO = JsonConvert.DeserializeObject<MessageAccountDTO>(message);
            //    var email = new EmailSender();
            //    await email.SendEmailAsync(messageAccountDTO.Email, "Welcome To Contento System", "Username: " + messageAccountDTO.FullName + ", Password: " + messageAccountDTO.Password);
            //};
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            return Task.CompletedTask;
        }
    }

}
