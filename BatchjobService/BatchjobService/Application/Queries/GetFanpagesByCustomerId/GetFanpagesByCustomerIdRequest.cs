using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetFanpagesByCustomerId
{
    public class GetFanpagesByCustomerIdRequest : IRequest<List<FanpageViewModel>>
    {
        public int CustomerId { get; set; }
    }
}
