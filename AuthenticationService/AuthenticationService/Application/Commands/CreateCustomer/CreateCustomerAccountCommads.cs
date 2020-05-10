using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.CreateCustomer
{
    public class CreateCustomerAccountCommads : IRequest<CreateUserModel>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int IdMarketer { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CompanyName { get; set; }
    }
}
