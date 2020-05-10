using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.RecommendTimePublishOneMonth
{
    public class RecommendTimePublishOneMonthRequest : IRequest<List<RecommendsTimeModels>>
    {
        public List<int> IdFanpages { get; set; }
    }
}
