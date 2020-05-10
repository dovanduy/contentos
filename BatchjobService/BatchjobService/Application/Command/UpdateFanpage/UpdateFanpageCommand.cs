using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.UpdateFanpage
{
    public class UpdateFanpageCommand : IRequest<FanpageViewModel>
    {
        public int FanpageId { get; set; } = 0;
        public int ChannelId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public string Name { get; set; }
        public string Token { get; set; }
        public List<int> Tags { get; set; }
        public string Link { get; set; }
    }
}
