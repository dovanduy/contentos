using System;
using System.Collections.Generic;

namespace BatchjobService.Entities
{
    public partial class FanpagesInteraction
    {
        public int Id { get; set; }
        public int? IdFanpages { get; set; }
        public int? Interaction { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Fanpages IdFanpagesNavigation { get; set; }
    }
}
