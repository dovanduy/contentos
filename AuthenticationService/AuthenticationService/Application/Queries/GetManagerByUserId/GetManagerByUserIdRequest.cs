using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetManagerByUserId
{
    public class GetManagerByUserIdRequest : IRequest<ManagerModel>
    {
        public int id { get; set; }
    }
}
