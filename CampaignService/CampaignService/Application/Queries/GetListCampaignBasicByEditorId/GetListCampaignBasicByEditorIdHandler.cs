using CampaignService.Entities;
using CampaignService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignBasicByEditorId
{
    public class GetListCampaignBasicByEditorIdHandler : IRequestHandler<GetListCampaignBasicByEditorIdRequest, List<CampaignModels>>
    {
        private readonly ContentoDbContext _context;
        public GetListCampaignBasicByEditorIdHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<List<CampaignModels>> Handle(GetListCampaignBasicByEditorIdRequest request, CancellationToken cancellationToken)
        {
            return await _context.Campaigns.AsNoTracking().Where(x => x.IdEditor == request.IdEditor).Select(x => new CampaignModels
            {
                Id = x.Id,
                Name = x.Title
            }).ToListAsync();
        }
    }
}