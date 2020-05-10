using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Models
{
    public class StatisticsModel
    {
        public int IdTags { get; set; }
        public DateTime Date { get; set; }
        public int TimeInTeraction { get; set; }
      
    }
    public class StatisticsModelMonth
    {
        public int IdTags { get; set; }
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TimeInTeraction { get; set; }

    }
    public class WeekStatics
    {
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class StatisticMonthReturnModel
    {
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Date { get; set; }
        public List<int> TimeInteraction { get; set; }
    }
    public class StatisticReturnModel
    {
        public DateTime Date { get; set; }
        public List<int> TimeInteraction { get; set; }
    }

    public class ListTaskModel
    {
        public int IdTask { get; set; }
        public int TimeInTeraction { get; set; }
    }
    public class CountTask
    {
        public string Tag { get; set; }
        public int Task { get; set; }
    }
    public class ListTaskStaticModel
    {
        public int IdTask { get; set; }
        public string Title { get; set; }
        public int View { get; set; }
        public bool Published { get; set; }
    }
    public class TaskTrendViewStaicModel
    {
        public int IdTask { get; set; }
        public string Title { get; set; }
        public int View { get; set; }
    }
    public class ListTaskInteractionModel
    {
        public int IdTask { get; set; }
        public int View { get; set; }

    }
    public class StaticsDetailModel
    {
        public DateTime? Date { get; set; }
        public int TimeInTeraction { get; set; }
    }

}
