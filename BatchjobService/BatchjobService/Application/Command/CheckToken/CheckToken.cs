using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.CheckToken
{
    public class CheckToken
    {
        public int ChannelId { get; set; }
        public string Token { get; set; }
    }
}
