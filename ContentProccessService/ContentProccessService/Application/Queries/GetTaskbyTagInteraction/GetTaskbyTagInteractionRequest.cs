using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskbyTagInteraction
{
    public class GetTaskbyTagInteractionRequest : IRequest<List<StaticsDetailModel>>
    {
        public int Id { get; set; }
    }
}
