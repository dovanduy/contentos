using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetProfile
{
    public class GetProfileRequest : IRequest<UserProfileModels>
    {
        [Required]
        public int IdUser { get; set; }
    }
}
