using System;
using System.Collections.Generic;

namespace ContentProccessService.Entities
{
    public partial class Comments
    {
        public int Id { get; set; }
        public int? IdContent { get; set; }
        public string Comment { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Contents IdContentNavigation { get; set; }
    }
}
