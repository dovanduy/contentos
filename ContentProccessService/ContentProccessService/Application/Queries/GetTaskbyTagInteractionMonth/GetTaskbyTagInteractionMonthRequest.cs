using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskbyTagInteractionMonth
{
    public class GetTaskbyTagInteractionMonthRequest : IRequest<List<StaticsDetailModel>>
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
