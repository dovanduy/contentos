using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            Accounts = new HashSet<Accounts>();
        }

        public int Id { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Accounts> Accounts { get; set; }
    }
}
