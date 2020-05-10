using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerAccountCommads : IRequest<CreateUserModel>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CompanyName { get; set; }
    }
}
