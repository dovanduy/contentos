using System;
using System.Collections.Generic;

namespace ContentProccessService.Entities
{
    public partial class StatusCampaigns
    {
        public StatusCampaigns()
        {
            Campaigns = new HashSet<Campaigns>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Campaigns> Campaigns { get; set; }
    }
}
