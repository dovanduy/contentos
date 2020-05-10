using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListWriterBasic
{
    public class GetListWriterBasicRequest : IRequest<List<ListUserModel>>
    {
    }
}
