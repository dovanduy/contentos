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

namespace BatchjobService.Application.Queries.GetInteractionByCampaignId
{
    public class GetInteractionByCampaignIdHandler : IRequestHandler<GetInteractionByCampaignIdRequest, FacebookInteractionModel>
    {
        private readonly ContentoDbContext _context;

        public GetInteractionByCampaignIdHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<FacebookInteractionModel> Handle(GetInteractionByCampaignIdRequest request, CancellationToken cancellationToken)
        {
            var taskFanpages = _context.TasksFanpages.Include(i => i.IdFanpageNavigation).Include(i => i.IdTaskNavigation)
                .Where(w => w.IdFanpageNavigation.IdChannel == 2 && w.IdTaskNavigation.IdCampaign == request.campaignId).ToList();

            var campaign = _context.Campaigns.Find(request.campaignId);
            Dictionary<string, List<FacebookInteraction>> map = new Dictionary<string, List<FacebookInteraction>>();

            foreach (var taskFanpage in taskFanpages)
            {
                if (!map.ContainsKey(taskFanpage.IdFanpageNavigation.Name))
                {
                    map.Add(taskFanpage.IdFanpageNavigation.Name, new List<FacebookInteraction>());
                }

                var lst = map.GetValueOrDefault(taskFanpage.IdFanpageNavigation.Name);

                var interaction = JObject.Parse(await Helper.GetInteraction(taskFanpage.IdFacebook, taskFanpage.IdFanpageNavigation.Token));

                int possitiveCommentCount = 0;

                if (interaction["comments"]["data"].HasValues)
                {
                    var comments = interaction["comments"]["data"];
                    foreach(var comment in comments)
                    {
                        string message = comment["message"].Value<string>().ToLower();

                        if (message.Equals("."))
                        {
                            possitiveCommentCount++;
                        }
                        else
                        {
                            if (message.Contains("inbox"))
                            {
                                possitiveCommentCount++;
                            }
                        }
                    }
                }

                string shareCount;

                if (!interaction.ContainsKey("shares"))
                {
                    shareCount = "0";
                }
                else
                {
                    shareCount = interaction["shares"]["count"].Value<string>();
                }

                lst.Add(new FacebookInteraction
                {
                    title = taskFanpage.IdTaskNavigation.Title,
                    publicDate = taskFanpage.IdTaskNavigation.PublishTime,
                    link = "https://www.facebook.com/" + taskFanpage.IdFacebook,
                    shareCount = shareCount,
                    reactiontCount = interaction["reactions"]["summary"]["total_count"].Value<string>(),
                    commentCount = interaction["comments"]["summary"]["total_count"].Value<string>(),
                    possitiveCommentCount = possitiveCommentCount
                }
                );

                map[taskFanpage.IdFanpageNavigation.Name] = lst;
            }

            List<FacebookInteractionModel2> lstInt = new List<FacebookInteractionModel2>();
            foreach (var item in map)
            {
                lstInt.Add(new FacebookInteractionModel2 { name = item.Key, data = item.Value });
            }

            FacebookInteractionModel result = new FacebookInteractionModel { data = lstInt, startDate = campaign.StartDate, endDate = campaign.EndDate };

            return result;
        }
    }
}
