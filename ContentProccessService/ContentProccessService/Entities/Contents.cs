using System;
using System.Collections.Generic;

namespace ContentProccessService.Entities
{
    public partial class Contents
    {
        public Contents()
        {
            Comments = new HashSet<Comments>();
        }

        public int Id { get; set; }
        public int? IdTask { get; set; }
        public string Name { get; set; }
        public string TheContent { get; set; }
        public int? Version { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Tasks IdTaskNavigation { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
