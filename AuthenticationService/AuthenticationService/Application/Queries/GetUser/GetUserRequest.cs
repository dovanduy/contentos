using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetUser
{
    public class GetUserRequest : IRequest<List<ListUserModel>>
    {
        public int IdMarketer { get; set; }
    }
}
