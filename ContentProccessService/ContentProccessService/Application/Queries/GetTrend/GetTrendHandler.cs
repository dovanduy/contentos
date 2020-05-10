using ContentProccessService.Entities;
using ContentProccessService.Models;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTrend
{
    public class GetTrendHandler : IRequestHandler<GetTrendRequest, List<ContentViewer>>
    {
        private readonly ContentoDbContext _context;

        public GetTrendHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContentViewer>> Handle(GetTrendRequest request, CancellationToken cancellationToken)
        {
            //var contents = await _context.Tasks.AsNoTracking()
            //    .Include(c => c.)
            //    .Include(c => c.IdTaskNavigation.TasksFanpages)
            //    .Include(c => c.IdTaskNavigation.TasksTags).ThenInclude(TasksTags => TasksTags.IdTagNavigation)
            //    .Where(c => c.IdTaskNavigation.TasksFanpages.Any(t => t.IdFanpage == request.IdFanpage))
            //    .Where(c => c.IsActive == true)
            //    .Where(c => c.Interaction != 0)
            //    .OrderByDescending(c => c.Interaction).Take(20).ToListAsync();

            var contents=  await _context.Tasks.AsNoTracking()
              .Include(x => x.TasksTags).ThenInclude(TasksTags => TasksTags.IdTagNavigation)
              .Include(x=>x.Statistics)
             .Where(x => x.Status == 7
             && x.Contents.Any(t => t.IsActive == true)
             && x.TasksFanpages.Any(t => t.IdFanpage == 1)
             && x.PublishTime > DateTime.UtcNow.AddDays(-7)
             && x.PublishTime <= DateTime.UtcNow
             && x.Statistics.Sum(z=>z.Views) != 0)
             .OrderByDescending(x => x.Statistics.Sum(z => z.Views)).Take(10)
             .Select(x => new
             {
                 x,
                 Contents = x.Contents.Where(c => c.IsActive == true).FirstOrDefault(),
                 TasksTags = x.TasksTags.ToList()
             }).ToListAsync();
            var lstContentReturn = new List<ContentViewer>();
            foreach (var item in contents)
            {
                List<string> imgs = getImage(item.Contents.TheContent);
                if (imgs.Count == 0)
                {
                    imgs.Add("https://marketingland.com/wp-content/ml-loads/2015/11/content-marketing-idea-lightbulb-ss-1920.jpg");
                }
                var Cnt = new ContentModels
                {
                    Id = item.Contents.Id,
                    Name = item.Contents.Name
                };
                var lstTag = new List<TagsViewModel>();
                var lstintTag = new List<int>();
                foreach (var item1 in item.TasksTags)
                {
                    var Tag = new TagsViewModel
                    {
                        Id = item1.IdTag,
                        Name = item1.IdTagNavigation.Name
                    };
                    lstTag.Add(Tag);
                    lstintTag.Add(item1.IdTag);
                }

                var ContentReturn = new ContentViewer
                {
                    IdTask = item.x.Id,
                    PublishTime = item.x.PublishTime,
                    Contents = Cnt,
                    Image = imgs,
                    ListTags = lstTag,
                    ListIntTags = lstintTag
                };
                lstContentReturn.Add(ContentReturn);
            }
            return lstContentReturn;
        }
        public static List<string> getImage(string content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var imgs = doc.DocumentNode.Descendants("img");
            List<string> listImg = new List<string>();

            foreach (var img in imgs)
            {
                listImg.Add(img.GetAttributeValue("src", null));
            }

            return listImg;
        }
    }
}
