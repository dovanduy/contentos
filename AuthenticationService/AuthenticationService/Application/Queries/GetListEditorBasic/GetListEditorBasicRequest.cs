using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListEditorBasic
{
    public class GetListEditorBasicRequest :  IRequest<List<ListUserModel>>
    {
    }
}
