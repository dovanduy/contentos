using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetNotify
{
    public class GetNotifysRequest : IRequest<List<GetNotifyModel>>
    {
        [Required]
        public int UserId { get; set; }
    }
}
