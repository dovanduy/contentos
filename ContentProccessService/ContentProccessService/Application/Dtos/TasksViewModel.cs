using ContentProccessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Dtos
{
    public class TasksViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public int? IsActive { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public UsersModels Writer { get; set; }
        public UsersModels Editor { get; set; }
        public StatusModels Status { get; set; }
        public string Content { get; set; }
    }
    public class UsersModels
    {
        public int? Id { get; set; }
        public string Name { get; set; }

    }
    public class StatusModels
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

    }
}
