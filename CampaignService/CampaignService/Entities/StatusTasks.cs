using System;
using System.Collections.Generic;

namespace CampaignService.Entities
{
    public partial class StatusTasks
    {
        public StatusTasks()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
