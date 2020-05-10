using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetProfile
{
    public class GetProfileHandler : IRequestHandler<GetProfileRequest, UserProfileModels>
    {
        private readonly ContentoDbContext _context;

        public GetProfileHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<UserProfileModels> Handle(GetProfileRequest request, CancellationToken cancellationToken)
        {

            var profile = await _context.Accounts
                .AsNoTracking()
                .Include(x => x.IdUserNavigation)
                .ThenInclude(IdUserNavigation=> IdUserNavigation.Personalizations)
                .ThenInclude(Personalizations=> Personalizations.IdTagNavigation)
                .FirstOrDefaultAsync(x => x.IdUserNavigation.Id == request.IdUser);
            if (profile == null)
            {
                return null;
            }
            if (profile.IdRole  == 4)
            {
                var lstTags = new List<TagViewModels>();
                var lstIntTags = new List<int>();
                foreach (var item in profile.IdUserNavigation.Personalizations.Where(x=>x.IsChosen == true))
                {
                    var Tags = new TagViewModels
                    {
                        Id = item.IdTag,
                        Name = item.IdTagNavigation.Name
                    };
                    lstIntTags.Add(item.IdTag);
                    lstTags.Add(Tags);
                }
                var profileDetail = new UserProfileModels
                {
                    Id = profile.IdUserNavigation.Id,
                    Age = profile.IdUserNavigation.Age,
                    CompanyName = profile.IdUserNavigation.Company,
                    Email = profile.Email,
                    FirstName = profile.IdUserNavigation.FirstName,
                    LastName = profile.IdUserNavigation.LastName,
                    Gender = profile.IdUserNavigation.Gender,
                    Phone = string.IsNullOrEmpty(profile.IdUserNavigation.Phone) == true ? null : profile.IdUserNavigation.Phone.Trim(),
                    IdTags = lstIntTags,
                    Tags = lstTags
                };
                return profileDetail;
            }
            else
            {
                var profileDetail = new UserProfileModels
                {
                    Id = profile.IdUserNavigation.Id,
                    Age = profile.IdUserNavigation.Age,
                    CompanyName = profile.IdUserNavigation.Company,
                    Email = profile.Email,
                    FirstName = profile.IdUserNavigation.FirstName,
                    LastName = profile.IdUserNavigation.LastName,
                    Gender = profile.IdUserNavigation.Gender,
                    Phone = string.IsNullOrEmpty(profile.IdUserNavigation.Phone) == true ? null : profile.IdUserNavigation.Phone.Trim(),
                };
                return profileDetail;
            }

        }
    }
}
