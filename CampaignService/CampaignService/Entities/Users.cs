using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class Users
    {
        public Users()
        {
            Accounts = new HashSet<Accounts>();
            CampaignsIdCustomerNavigation = new HashSet<Campaigns>();
            CampaignsIdEditorNavigation = new HashSet<Campaigns>();
            CampaignsIdMarketerNavigation = new HashSet<Campaigns>();
            Fanpages = new HashSet<Fanpages>();
            InverseIdManagerNavigation = new HashSet<Users>();
            Personalizations = new HashSet<Personalizations>();
            Tasks = new HashSet<Tasks>();
            Tokens = new HashSet<Tokens>();
            UsersInteractions = new HashSet<UsersInteractions>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public bool? IsActive { get; set; }
        public int? IdManager { get; set; }

        public virtual Users IdManagerNavigation { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Campaigns> CampaignsIdCustomerNavigation { get; set; }
        public virtual ICollection<Campaigns> CampaignsIdEditorNavigation { get; set; }
        public virtual ICollection<Campaigns> CampaignsIdMarketerNavigation { get; set; }
        public virtual ICollection<Fanpages> Fanpages { get; set; }
        public virtual ICollection<Users> InverseIdManagerNavigation { get; set; }
        public virtual ICollection<Personalizations> Personalizations { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Tokens> Tokens { get; set; }
        public virtual ICollection<UsersInteractions> UsersInteractions { get; set; }
    }
}
