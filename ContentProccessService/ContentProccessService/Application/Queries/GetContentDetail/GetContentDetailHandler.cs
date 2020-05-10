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

namespace ContentProccessService.Application.Queries.GetContentDetail
{
    public class GetContentDetailHandler : IRequestHandler<GetContentDetailRequest, ContentDetailReturn>
    {
        private readonly ContentoDbContext _context;
        public GetContentDetailHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<ContentDetailReturn> Handle(GetContentDetailRequest request, CancellationToken cancellationToken)
        {
            var content = await _context.Tasks.AsNoTracking()
              .Include(x=>x.IdWritterNavigation)
              .Include(x=>x.TasksTags).ThenInclude(TasksTags=> TasksTags.IdTagNavigation)
              .Where(x => x.Id == request.IdTask)
              .Select(x => new
              {
                  x,
                  Contents = x.Contents.Where(c => c.IsActive == true).FirstOrDefault(),
                 //TasksTags = x.TasksTags.ToList(),
                  
              }).FirstOrDefaultAsync();
            List<string> imgs = getImage(content.Contents.TheContent);
            if (imgs.Count == 0)
            {
                imgs.Add("https://marketingland.com/wp-content/ml-loads/2015/11/content-marketing-idea-lightbulb-ss-1920.jpg");
            }
            var Cnt = new ContentModels
            {
                Id = content.Contents.Id,
                Content = content.Contents.TheContent,
                Name = content.Contents.Name
            };
            var Writter = new UsersModels
            {
                Id = content.x.IdWritter,
                Name = content.x.IdWritterNavigation.FirstName + " " + content.x.IdWritterNavigation.LastName
            };
            var lstTag = new List<TagsViewModel>();
            foreach (var item in content.x.TasksTags)
            {
                var Tag = new TagsViewModel
                {
                    Id = item.IdTag,
                    Name = item.IdTagNavigation.Name


                };
                lstTag.Add(Tag);
            }
            var ContentReturn = new ContentDetailReturn
            {
                IdTask = content.x.Id,
                PublishTime = content.x.PublishTime,
                Contents = Cnt,
                Image = imgs,
                ListTags = lstTag,
                Writer = Writter
            };
            return ContentReturn;
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
