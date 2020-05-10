using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateListTaskChannel
{
    public class CreateListTaskChannelRequest : IRequest<List<TaskChannelModelRespone>>
    {
       public List<TaskChannelModel> ListTaskChannel { get; set; }
    }

}

