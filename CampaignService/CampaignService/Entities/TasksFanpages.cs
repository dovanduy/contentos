using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class TasksFanpages
    {
        public int IdTask { get; set; }
        public int IdFanpage { get; set; }
        public string IdJob { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string IdFacebook { get; set; }

        public virtual Fanpages IdFanpageNavigation { get; set; }
        public virtual Tasks IdTaskNavigation { get; set; }
    }
}
