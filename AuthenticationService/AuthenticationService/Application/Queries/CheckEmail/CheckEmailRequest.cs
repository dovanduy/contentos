using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.CheckEmail
{
    public class CheckEmailRequest : IRequest<bool>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
