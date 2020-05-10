using MediatR;
using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetWriter
{
    public class GetWriterRequest : IRequest<List<ListUserModel>>
    {
        public int EditorId { get; set; }
    }
}
