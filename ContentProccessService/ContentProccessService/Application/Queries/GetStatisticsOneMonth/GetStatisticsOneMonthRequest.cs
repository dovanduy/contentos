using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetStatisticsOneMonth
{
    public class GetStatisticsOneMonthRequest : IRequest<List<StatisticMonthReturnModel>>
    {
        public int Quantity { get; set; }
    }
}
