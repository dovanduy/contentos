using System;
using System.Collections.Generic;

namespace BatchjobService.Entities
{
    public partial class Accounts
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdRole { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Roles IdRoleNavigation { get; set; }
        public virtual Users IdUserNavigation { get; set; }
    }
}
