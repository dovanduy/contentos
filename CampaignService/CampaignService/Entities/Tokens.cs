using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class Tokens
    {
        public Tokens()
        {
            Notifys = new HashSet<Notifys>();
        }

        public int Id { get; set; }
        public int? IdUser { get; set; }
        public string Token { get; set; }
        public string DeviceType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Users IdUserNavigation { get; set; }
        public virtual ICollection<Notifys> Notifys { get; set; }
    }
}
