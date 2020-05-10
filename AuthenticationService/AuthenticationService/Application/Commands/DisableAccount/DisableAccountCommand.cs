using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.DisableAccount
{
    public class DisableAccountCommand : IRequest<string>
    {
        public int proAccount { get; set; }
        public int reicAccount { get; set; }
    }
}
