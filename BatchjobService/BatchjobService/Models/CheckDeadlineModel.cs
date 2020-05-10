using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Models
{
    public class CheckDeadlineModel
    {
        public string Email { get; set; }
        public List<string> ListTask { get; set; }

    }
}
