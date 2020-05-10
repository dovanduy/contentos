using System;
using System.Collections.Generic;

namespace ContentProccessService.Entities
{
    public partial class Notifys
    {
        public int Id { get; set; }
        public int? IdToken { get; set; }
        public string Title { get; set; }
        public string Messager { get; set; }
        public DateTime? Date { get; set; }

        public virtual Tokens IdTokenNavigation { get; set; }
    }
}
