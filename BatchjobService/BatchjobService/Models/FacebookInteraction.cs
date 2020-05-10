using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class FacebookInteraction
    {
        public string title { get; set; }
        public DateTime? publicDate { get; set; }
        public string reactiontCount { get; set; }
        public string shareCount { get; set; }
        public string commentCount { get; set; }
        public string link { get; set; }

        public int possitiveCommentCount { get; set; }
    }

    public class FacebookInteractionModel2
    {
        public string name { get; set; }
        public List<FacebookInteraction> data { get; set; }
    }

    public class FacebookInteractionModel
    {
        public List<FacebookInteractionModel2> data { get; set; }

        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }
    }

    public class FacebookPageStatisticsModel
    {
        public string name { get; set; }
        public int newLikeCount { get; set; }
        public int newInboxCount { get; set; }
    }

    public class FacebookPageStatistics
    {
        public string name { get; set; }
        public int interaction { get; set; }
        public int inbox { get; set; }
        public int view { get; set; }
        public int reaction { get; set; }
        public int comment { get; set; }
        public int share { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public Status status { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }
}
