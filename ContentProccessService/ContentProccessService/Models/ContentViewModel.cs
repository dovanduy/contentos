using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Models
{
    public class ContentViewModel
    {
        public int Id { get; set; }
        public Task Task { get; set; } = new Task();
        public string Name { get; set; }
        public string TheContent { get; set; }
        public int? Version { get; set; }
    }

    public class Task
    {
        public int? Id { get; set; }
        public string Title { get; set; }
    }
}
