using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class RegisterAccountCommands : IRequest<int>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public string Phone { get; set; }
        public List<int> Tags { get; set; }

    }
}
