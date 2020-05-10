using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.Test
{
    public class TestRequest : IRequest<List<AlgorithmDataBeforeModel>>
    {
    }
}
