using BatchjobService.Entities;
using BatchjobService.Utulity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.HangFireService
{
    public interface ICountInteraction
    {
        Task CountAsync();
    }

    public class CountInteraction : ICountInteraction
    {
        private readonly ContentoDbContext _context;

        public CountInteraction(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task CountAsync()
        {
            var taskFanpages = _context.TasksFanpages.Include(i => i.IdFanpageNavigation).Include(i => i.IdTaskNavigation)
                .Where(w => w.IdFanpageNavigation.IdChannel == 2).ToList();

            Dictionary<int, int> map = new Dictionary<int, int>();

            foreach (var taskFanpage in taskFanpages)
            {
                if (!map.ContainsKey(taskFanpage.IdFanpage))
                {
                    map.Add(taskFanpage.IdFanpage, 0);
                }

                var lst = map.GetValueOrDefault(taskFanpage.IdFanpage);

                var interaction = JObject.Parse(await Helper.GetInteraction(taskFanpage.IdFacebook, taskFanpage.IdFanpageNavigation.Token));

                int count = 0;

                count += interaction["reactions"]["summary"]["total_count"].Value<int>();
                count += interaction["comments"]["summary"]["total_count"].Value<int>();

                if (!interaction.ContainsKey("shares"))
                {
                    count += 0;
                }
                else
                {
                    count += interaction["shares"]["count"].Value<int>();
                }

                

                map[taskFanpage.IdFanpage] += count;
            }
            List<FanpagesInteraction> result = new List<FanpagesInteraction>();

            foreach (var item in map)
            {
                result.Add(new FanpagesInteraction { IdFanpages = item.Key, Interaction = item.Value, CreatedDate = DateTime.UtcNow });
            }

            _context.AddRange(result);
            _context.SaveChanges();
        }

    }
}
