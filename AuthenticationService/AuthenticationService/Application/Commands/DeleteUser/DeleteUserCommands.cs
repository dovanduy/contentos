using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.DeleteUser
{
    public class DeleteUserCommands : IRequest<UserAdminModels>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
