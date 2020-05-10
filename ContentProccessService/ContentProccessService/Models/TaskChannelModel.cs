using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Models
{
    public class TaskChannelModel
    {
        public int IdTask { get; set; }
        public int IdChannel { get; set; }
    }

    public class TaskChannelModelRespone
    {
        public int id { get; set; }
        public int IdTask { get; set; }
        public int IdChannel { get; set; }
    }

}
