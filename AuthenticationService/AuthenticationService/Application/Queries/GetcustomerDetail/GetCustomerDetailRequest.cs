using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetcustomerDetail
{
    public class GetCustomerDetailRequest : IRequest<UserModelDetail>
    {
        public int IdCustomer { get; set; }
    }
}
