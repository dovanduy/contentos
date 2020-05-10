using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTrend
{
    public class GetTrendRequest : IRequest<List<ContentViewer>>
    {
    }
}
