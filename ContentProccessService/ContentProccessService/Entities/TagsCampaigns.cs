using System;
using System.Collections.Generic;

namespace ContentProccessService.Entities
{
    public partial class TagsCampaigns
    {
        public int IdCampaign { get; set; }
        public int IdTag { get; set; }

        public virtual Campaigns IdCampaignNavigation { get; set; }
        public virtual Tags IdTagNavigation { get; set; }
    }
}
