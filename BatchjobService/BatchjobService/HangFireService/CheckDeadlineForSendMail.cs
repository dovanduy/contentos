using BatchjobService.Entities;
using BatchjobService.Models;
using BatchjobService.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.HangFireService
{
    public interface ICheckDeadlineForSendMail
    {
        List<CheckDeadlineModel> CheckDeadlineForMail();
        void PublishMessage();
    }

    public class CheckDeadlineForSendMail : ICheckDeadlineForSendMail
    {

        private readonly ContentoDbContext _context;
        public CheckDeadlineForSendMail(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public  List<CheckDeadlineModel> CheckDeadlineForMail()
        {
            var lstTask =  _context.Tasks
                .Include(x=>x.IdWritterNavigation)
                .ThenInclude(IdWritterNavigation=> IdWritterNavigation.Accounts)
                .Where(x => x.Deadline >= DateTime.UtcNow 
                && x.Deadline < DateTime.UtcNow.AddDays(1) 
                && (x.Status == 1 || x.Status == 2))
                .ToList();
            var lstCheckModle = new List<CheckDeadlineModel>();
            if (lstTask.Count != 0)
            {
                var lstUser = lstTask.Select(x => x.IdWritter).Distinct().ToList();
               
                foreach (var item in lstUser)
                {
                    var checkModle = new CheckDeadlineModel();
                    checkModle.Email = _context.Accounts.FirstOrDefault(x => x.IdUserNavigation.Id == item).Email;
                    var lstTitile = new List<string>();
                    foreach (var item1 in lstTask)
                    {
                        if (item1.IdWritter == item)
                        {
                            var title = item1.Title;
                            lstTitile.Add(title);
                        }

                    }
                    checkModle.ListTask = lstTitile;
                    lstCheckModle.Add(checkModle);
                }
                return lstCheckModle;
            }
            return lstCheckModle;
        }
        public void PublishMessage()
        {
            //Create exchange
            Producer producer = new Producer();
            var data =  CheckDeadlineForMail();
            var TaskDeadlineDTO = new List<CheckDeadlineModel>();
            TaskDeadlineDTO = data;
            producer.PublishMessage(message: JsonConvert.SerializeObject(TaskDeadlineDTO), "DedlineTask");
        }


    }
}
