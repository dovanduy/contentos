using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetContentViewer
{
    public class GetContentViewerRequest : IRequest<List<ContentViewer>>
    {
        public List<int> Tags { get; set; }
        public int? Id { get; set; }
    }
}
