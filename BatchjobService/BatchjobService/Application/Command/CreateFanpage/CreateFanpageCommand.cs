using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.CreateFanpage
{
    public class CreateFanpageCommand : IRequest<FanpageViewModel>
    {
        public int ChannelId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public int MarketerId { get; set; } = 0;
        public string Name { get; set; }
        public string Token { get; set; }
        public List<int> Tags { get; set; }

        public string Link { get; set; }
    }
}
