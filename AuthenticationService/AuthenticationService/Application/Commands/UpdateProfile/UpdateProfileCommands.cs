using AuthenticationService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.UpdateProfile
{
    public class UpdateProfileCommands : IRequest<UserProfileModels>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public List<int> IdTags { get; set; }
    }
}
