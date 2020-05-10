using AuthenticationService.Models;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries
{
    public class AuthenticationRequest : IRequest<LoginSuccessViewModel> 
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
