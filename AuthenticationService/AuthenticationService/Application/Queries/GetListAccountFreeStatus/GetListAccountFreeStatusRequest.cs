using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;

namespace AuthenticationService.Application.Queries.GetListAccountFreeStatus
{
    public class GetListAccountFreeStatusRequest : IRequest<List<UserAdminModels>>
    {
    }
}
