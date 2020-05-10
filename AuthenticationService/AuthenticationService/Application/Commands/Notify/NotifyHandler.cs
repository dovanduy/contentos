using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net;
using AuthenticationService.Models;
using AuthenticationService.Entities;
using AuthenticationService.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace AuthenticationService.Application.Commands.Notify
{
    public class NotifyHandler : IRequestHandler<NotifyCommands, NotifyModel>
    {
        private readonly ContentoDbContext _context;

        public NotifyHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<NotifyModel> Handle(NotifyCommands request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var token = await GetTokenAsync(request.Reciever);
                if (token != null)
                {
                    bool isSendNotify = SendNotify(request,token);
                    if (isSendNotify)
                    {
                        var newNotify = new Notifys
                        {
                            //Sender = request.Sender,
                            //Reciever = request.Reciever,
                            IdToken = token.Id,
                            Messager = request.Messenger,
                            Title = request.Title,
                            Date = DateTime.UtcNow

                        };

                        _context.Notifys.Add(newNotify);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return new NotifyModel
                        {
                            Id = _context.Notifys.AsNoTracking().OrderByDescending(x => x.Id).First().Id,
                            Title = newNotify.Title,
                            Messager = newNotify.Messager,
                            // đợi chỉnh sửa database
                            //Reviever = newNotify.Reciever,
                            //Sender = newNotify.Sender,
                            //TokenId = newNotify.TokenId,
                            //IdObject = newNotify.IdObject,
                            //TypeSend = newNotify.TypeSend
                        };
                    }
                    return null;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }
        }

        public async Task<Tokens> GetTokenAsync(int RecieverId)
        {
            var token = await _context.Tokens.AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdUser == RecieverId);
            return token;

        }

        public bool SendNotify(NotifyCommands request, Tokens tokens)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAAHxqgzU:APA91bFCTHWFUIrq-aEFuP7uepcpnF_IRQF6RRZ2taQIzw-ns51gqnc1u6uvEFaTnYsLpH0goSwn4gMOciOcAuRyaPuuk6Z-2ttJ09oquYQ9JYYZC8pDPVOQ3EYTyqRwYWBXE25HKy01"));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", "2087355189"));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    //to = "cKddtT0riAU:APA91bHks4sUqORwyF3c0gCaTWaLJbSUNe2CLCyj4yqPaWD5woBoPCqXHulDsHiA7zrDMJ5gK_vZ6UNObq_hX3oKQGz6v9G1N1nKP93XsTJCdODohyGBkNjSIMkcqZo8GL8CFKuTZun6",
                    to = tokens.Token,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = request.Messenger,
                        title = request.Title,
                        badge = 1
                    },
                    //data = new
                    //{
                    //    image = "Conmeongu",
                    //},

                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    return true;
                                }
                            else
                            {
                                return false;
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                string str = ex.Message;
            }
            return false;
        }
    }
}
