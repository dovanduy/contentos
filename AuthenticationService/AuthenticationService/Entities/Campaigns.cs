using System;
using System.Collections.Generic;

namespace AuthenticationService.Entities
{
    public partial class Campaigns
    {
        public Campaigns()
        {
            TagsCampaigns = new HashSet<TagsCampaigns>();
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdMarketer { get; set; }
        public int? IdEditor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? StartDate { get; set; }

        public virtual Users IdCustomerNavigation { get; set; }
        public virtual Users IdEditorNavigation { get; set; }
        public virtual Users IdMarketerNavigation { get; set; }
        public virtual StatusCampaigns StatusNavigation { get; set; }
        public virtual ICollection<TagsCampaigns> TagsCampaigns { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
