using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.Models
{
    public class TasksViewModelMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Campaign { get; set; }
        public string EmailWriter { get; set; }
    }
}
