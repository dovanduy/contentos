using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.CreateUser
{
    public class CreateUserCommands : IRequest<UserAdminModels>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public int Role { get; set; }
        public List<int> IdMarketer { get; set; }
        public List<int> IdEditor { get; set; }
        public List<int> IdWriter { get; set; }
    }
}
