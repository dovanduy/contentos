using BatchjobService.Entities;
using BatchjobService.Models;
using BatchjobService.Utulity;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetPageStatistics
{
    public class GetPageStatisticsHandler : IRequestHandler<GetPageStatisticsRequest, List<FacebookPageStatisticsModel>>
    {
        private readonly ContentoDbContext _context;

        public GetPageStatisticsHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<FacebookPageStatisticsModel>> Handle(GetPageStatisticsRequest request, CancellationToken cancellationToken)
        {
            var fanpages = _context.Fanpages.Where(w => w.IdCustomer == request.customerId).ToList();

            List<FacebookPageStatisticsModel> result = new List<FacebookPageStatisticsModel>();

            foreach (var fanpage in fanpages)
            {
                var id = fanpage.Link.Substring(fanpage.Link.LastIndexOf("/" + 1));
                FacebookPageStatisticsModel model = new FacebookPageStatisticsModel();

                model.name = fanpage.Name;

                var rezConversation = JObject.Parse(await Helper.GetConversation(id, fanpage.Token));

                int conversationCount = 0;
                if (rezConversation["data"].HasValues)
                {
                    var conversations = rezConversation["data"];

                    foreach (var conversation in conversations)
                    {
                        if(DateTime.Parse(conversation["updated_time"].Value<string>()) < fanpage.ModifiedDate)
                        {
                            break;
                        }
                        conversationCount++;
                    }
                }

                model.newInboxCount = conversationCount;

                var rezLike = JObject.Parse(await Helper.GetNewLike(id, fanpage.Token, fanpage.ModifiedDate));

                int likeCount = 0;
                var likes = rezLike["data"][0]["values"];

                foreach (var like in likes)
                {
                    likeCount += like["value"].Value<int>();
                }

                model.newLikeCount = likeCount;

                result.Add(model);
            }

            return result;
        }
    }
}
