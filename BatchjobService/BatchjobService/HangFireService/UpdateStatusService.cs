using BatchjobService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.HangFireService

{
    public interface  IUpdateStatusService
    {
        void UpdateStatus();
    }
    public class UpdateStatusService : IUpdateStatusService
    {
        private readonly ContentoDbContext _context;
        public UpdateStatusService(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public  void UpdateStatus()
        {
            var lstTask = _context.Tasks;
            var lstTaskUpdateStatus = lstTask.Where(x => x.Status == 2 || x.Status == 1);
            foreach (var item in lstTaskUpdateStatus)
            {
                if (item.Deadline < DateTime.UtcNow)
                {
                    item.Status = 4;
                    item.ModifiedDate = DateTime.UtcNow;
                }
            }
            var lstTaskUpdateIsAds = lstTask.Where(x => x.Status == 7 && x.IsAds == true);
            foreach (var item in lstTaskUpdateIsAds)
            {
                if(item.AdsDate < DateTime.UtcNow)
                {
                    item.IsAds = false;
                    item.AdsDate = null;
                }
            }
            var lstTaskUpdateOverduePublish = lstTask.Where(x => x.Status == 5);
            foreach (var item in lstTaskUpdateOverduePublish)
            {
                if (item.PublishTime < DateTime.UtcNow)
                {
                    item.Status = 4;
                }
            }
            _context.UpdateRange(lstTaskUpdateStatus);
            _context.UpdateRange(lstTaskUpdateIsAds);
            _context.UpdateRange(lstTaskUpdateOverduePublish);
            _context.SaveChanges();
        }
    }
}
