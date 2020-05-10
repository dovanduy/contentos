using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetContentViewerRecommend
{
    public class GetContentViewerRecommendRequest  : IRequest<List<ContentViewer>>
    {
        public int Id { get; set; }
    }
}
