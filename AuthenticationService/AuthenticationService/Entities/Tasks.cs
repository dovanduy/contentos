using System;
using System.Collections.Generic;

namespace AuthenticationService.Entities
{
    public partial class Tasks
    {
        public Tasks()
        {
            Contents = new HashSet<Contents>();
            Statistics = new HashSet<Statistics>();
            TasksFanpages = new HashSet<TasksFanpages>();
            TasksTags = new HashSet<TasksTags>();
            UsersInteractions = new HashSet<UsersInteractions>();
        }

        public int Id { get; set; }
        public int? IdCampaign { get; set; }
        public int? IdWritter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? AdsDate { get; set; }
        public bool? IsAds { get; set; }
        public DateTime? PublishTime { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Campaigns IdCampaignNavigation { get; set; }
        public virtual Users IdWritterNavigation { get; set; }
        public virtual StatusTasks StatusNavigation { get; set; }
        public virtual ICollection<Contents> Contents { get; set; }
        public virtual ICollection<Statistics> Statistics { get; set; }
        public virtual ICollection<TasksFanpages> TasksFanpages { get; set; }
        public virtual ICollection<TasksTags> TasksTags { get; set; }
        public virtual ICollection<UsersInteractions> UsersInteractions { get; set; }
    }
}
