using MediatR;
using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetCustomer
{
    public class GetCustomerRequest : IRequest<List<CreateUserModel>>
    {
        public int MarketerId { get; set; }
    }
}
