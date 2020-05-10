using BatchjobService.Entities;
using BatchjobService.Models;
using BatchjobService.Utulity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetCountInteractionByCampaignId
{
    public class GetCountInteractionByCampaignIdHandler : IRequestHandler<GetCountInteractionByCampaignIdRequest, FacebookPageStatistics>
    {
        private readonly ContentoDbContext _context;

        public GetCountInteractionByCampaignIdHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<FacebookPageStatistics> Handle(GetCountInteractionByCampaignIdRequest request, CancellationToken cancellationToken)
        {
            var taskFanpages = _context.TasksFanpages.Include(i => i.IdFanpageNavigation).Include(i => i.IdTaskNavigation)
               .Where(w => w.IdFanpageNavigation.IdChannel == 2 && w.IdTaskNavigation.IdCampaign == request.campaignId).ToList();

            var campaign = _context.Campaigns.Include(i => i.StatusNavigation).FirstOrDefault(f => f.Id == request.campaignId);

            int count = 0;

            int conversationCount = 0;

            int viewCount = 0;

            int reaction = 0;
            int comment = 0;
            int share = 0;

            List<int?> listFanpages = new List<int?>();
            foreach (var taskFanpage in taskFanpages)
            {
                var interaction = JObject.Parse(await Helper.GetInteraction(taskFanpage.IdFacebook, taskFanpage.IdFanpageNavigation.Token));
                count += interaction["reactions"]["summary"]["total_count"].Value<int>();
                count += interaction["comments"]["summary"]["total_count"].Value<int>();

                reaction += interaction["reactions"]["summary"]["total_count"].Value<int>();
                comment += interaction["comments"]["summary"]["total_count"].Value<int>();

                if (!interaction.ContainsKey("shares"))
                {
                    count += 0;
                    share += 0;
                }
                else
                {
                    count += interaction["shares"]["count"].Value<int>();
                    share += interaction["shares"]["count"].Value<int>();
                }


                if (!listFanpages.Contains(taskFanpage.IdFanpage))
                {
                    listFanpages.Add(taskFanpage.IdFanpage);

                    var id = taskFanpage.IdFanpageNavigation.Link.Substring(taskFanpage.IdFanpageNavigation.Link.LastIndexOf("/" + 1));

                    var rezConversation = JObject.Parse(await Helper.GetConversation(id, taskFanpage.IdFanpageNavigation.Token));

                    if (rezConversation["data"].HasValues)
                    {
                        var conversations = rezConversation["data"];

                        foreach (var conversation in conversations)
                        {
                            if (DateTime.Parse(conversation["updated_time"].Value<string>()) < taskFanpage.IdFanpageNavigation.ModifiedDate)
                            {
                                break;
                            }
                            conversationCount++;
                        }
                    }
                }

                var rezView = JObject.Parse(await Helper.GetView(taskFanpage.IdFacebook, taskFanpage.IdFanpageNavigation.Token));

                if (rezView["data"].HasValues)
                {
                    var view = rezView["data"];
                    viewCount += view[0]["values"][0]["value"].Value<int>();
                }
            }

            FacebookPageStatistics result = new FacebookPageStatistics
            {
                name = campaign.Title,
                interaction = count,
                inbox = conversationCount,
                view = viewCount,
                reaction = reaction,
                comment = comment,
                share = share,
                start_date = campaign.StartDate,
                end_date = campaign.EndDate,
                status = new Status { id = campaign.StatusNavigation.Id, name = campaign.StatusNavigation.Name, color = campaign.StatusNavigation.Color }
            };

            return result;
        }
    }
}
