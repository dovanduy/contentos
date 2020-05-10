using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class FanpageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Channel Channel { get; set; }
        public Customer Customer { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Token { get; set; } = "";
        public List<TagModel> Tags { get; set; }
        public List<int> TagId { get; set; }
        public string Link { get; set; }
    }

    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TagModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
