using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.CheckOldPassword
{
    public class CheckOldPasswordCommands : IRequest<bool>
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
    }
}
