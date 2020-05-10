using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetUserDetail
{
    public class GetUserDetailRequest : IRequest<UserDetailModels>
    {
        [Required]
        public int Id { get; set; }
    }
}
