using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListViewer
{
    public class GetListViewerRequest : IRequest<List<UserAdminModels>>
    {
    }
}
