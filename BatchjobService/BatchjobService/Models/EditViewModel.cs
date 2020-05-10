using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Channel { get; set; }
        public int Customer { get; set; } = 0;
        public DateTime? ModifiedDate { get; set; }
        public string Token { get; set; } = "";
        public List<TagModel> Tags { get; set; }
        public List<int> TagId { get; set; }
    }
}
