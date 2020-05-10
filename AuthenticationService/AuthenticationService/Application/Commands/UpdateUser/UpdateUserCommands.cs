using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.UpdateUser
{
    public class UpdateUserCommands : IRequest<UserAdminModels>
    {
        [Required]
        public int Id { get; set; }
        public string LastName { get; set; }
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
