using ContentProccessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Models
{
    public class ContentsViewModel
    {
        public int Id { get; set; }
        public Comments Comment { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
    public class ContentViewer
    {
        public int IdTask { get; set; }
        public ContentModels Contents { get; set; }
        public List<TagsViewModel> ListTags { get; set; }
        public List<string> Image { get; set; }
        public DateTime? PublishTime { get; set; }
        public List<int> ListIntTags { get; set; }
    }
    public class ContentDetailReturn
    {
        public int IdTask { get; set; }
        public ContentModels Contents { get; set; }
        public List<TagsViewModel> ListTags { get; set; }
        public List<string> Image { get; set; }
        public DateTime? PublishTime { get; set; }
        public UsersModels Writer { get; set; }
    }
}
