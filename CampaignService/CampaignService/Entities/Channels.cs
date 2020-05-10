using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class Channels
    {
        public Channels()
        {
            Fanpages = new HashSet<Fanpages>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Fanpages> Fanpages { get; set; }
    }
}
