using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.SaveToken
{
    public class SaveTokenCommands : IRequest<UserToken>
    {
        [Required]
        public int UserId;

        [Required]
        public string Token;

        [Required]
        public string DeviceType;


    }
}
