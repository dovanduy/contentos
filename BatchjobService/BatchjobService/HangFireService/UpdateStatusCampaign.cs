using BatchjobService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.HangFireService
{
    public interface IUpdateStatusCampaign
    {
        void UpdateStatus();
    }
    public class UpdateStatusCampaign : IUpdateStatusCampaign
    {
        private readonly ContentoDbContext _context;
        public UpdateStatusCampaign(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public void UpdateStatus()
        {
            var lstCampaignOverdue =  _context.Campaigns
                .Where(x=> (x.Status == 1 || x.Status == 2) 
                && x.Tasks.Any(y=>y.Status < 6) 
                && x.EndDate <= DateTime.UtcNow).ToList();
            foreach (var item in lstCampaignOverdue)
            {
                item.Status = 4;
                item.ModifiedDate = DateTime.UtcNow;
            }
            var lstCampaignSuccess = _context.Campaigns
              .Where(x => (x.Status == 1 || x.Status == 2)
              && x.Tasks.All(y => y.Status >= 6)
              && x.EndDate <= DateTime.UtcNow).ToList();
            foreach (var item in lstCampaignSuccess)
            {
                item.Status = 3;
                item.ModifiedDate = DateTime.UtcNow;
            }
            _context.UpdateRange(lstCampaignOverdue);
            _context.UpdateRange(lstCampaignSuccess);
            _context.SaveChanges();
        }

    }
}
