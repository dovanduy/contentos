using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class PublishModels
    {
        public List<int> listFanpage;
        public List<int> listTag;
        public int contentId;
        public DateTime time;
        public DateTime? adsTime;
        public bool isAds = false;
    }
}
