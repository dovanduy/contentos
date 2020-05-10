using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class Statistics
    {
        public int Id { get; set; }
        public int IdTask { get; set; }
        public int? Views { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Tasks IdTaskNavigation { get; set; }
    }
}
