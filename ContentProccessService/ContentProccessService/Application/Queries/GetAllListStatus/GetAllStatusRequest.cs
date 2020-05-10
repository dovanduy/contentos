
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetAllListStatus
{
    public class GetAllStatusRequest : IRequest<List<StatusModelsReturn>>
    {
    }
}
