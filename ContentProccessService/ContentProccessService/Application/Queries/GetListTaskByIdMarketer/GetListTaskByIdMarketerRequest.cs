using ContentProccessService.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetListTaskByIdMarketer
{
    public class GetListTaskByIdMarketerRequest : IRequest<List<TasksViewModel>>
    {
        [Required]
        public int IdMartketer { get; set; }
    }
}
