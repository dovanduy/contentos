using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class ContentModel
    { 
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? publish_time { get; set; }
        public List<Tag> listTag { get; set; }

        public bool? isAds { get; set; }

    }

    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
