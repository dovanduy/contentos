using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetContentViewerByTag
{
    public class GetContentViewerByTagRequest : IRequest<List<ContentViewer>>
    {
        public int IdTag { get; set; }
    }
}
