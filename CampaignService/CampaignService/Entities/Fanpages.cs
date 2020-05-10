using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class Fanpages
    {
        public Fanpages()
        {
            FanpagesInteraction = new HashSet<FanpagesInteraction>();
            FanpagesTags = new HashSet<FanpagesTags>();
            TasksFanpages = new HashSet<TasksFanpages>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? IdMarketer { get; set; }
        public int? IdChannel { get; set; }
        public string Token { get; set; }
        public bool? IsActive { get; set; }
        public int? IdCustomer { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Link { get; set; }

        public virtual Channels IdChannelNavigation { get; set; }
        public virtual Users IdMarketerNavigation { get; set; }
        public virtual ICollection<FanpagesInteraction> FanpagesInteraction { get; set; }
        public virtual ICollection<FanpagesTags> FanpagesTags { get; set; }
        public virtual ICollection<TasksFanpages> TasksFanpages { get; set; }
    }
}
