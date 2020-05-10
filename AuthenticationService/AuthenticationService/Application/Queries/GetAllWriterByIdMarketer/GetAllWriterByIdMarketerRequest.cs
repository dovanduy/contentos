using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetAllWriterByIdMarketer
{
    public class GetAllWriterByIdMarketerRequest : IRequest<List<ListUserModel>>
    {
        public int MarketerId { get; set; }
    }
}
