using System;
using System.Collections.Generic;

namespace AuthenticationService.Entities
{
    public partial class Statistics
    {
        public int Id { get; set; }
        public int IdTask { get; set; }
        public int? Views { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Tasks IdTaskNavigation { get; set; }
    }
}
