using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Commands.UpdateCampaign
{
    public class UpdateCampaignCommand : IRequest<CampaignData>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> Tags { get; set; }
        public Customer Customer { get; set; }
        public Editor Editor { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Editor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
