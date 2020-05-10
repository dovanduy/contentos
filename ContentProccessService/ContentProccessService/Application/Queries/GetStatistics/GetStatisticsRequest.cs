using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetStatistics
{
    public class GetStatisticsRequest : IRequest<List<StatisticReturnModel>>
    {
        public int Quantity { get; set; }
    }
}
