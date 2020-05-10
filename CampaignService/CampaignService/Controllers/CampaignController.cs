using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CampaignService.Application.Commands.CreateCampaign;
using CampaignService.Application.Commands.UpdateCampaign;
using CampaignService.Application.Queries.GetCampaign;
using CampaignService.Application.Queries.GetCampaignStatics;
using CampaignService.Application.Queries.GetListCampaign;
using CampaignService.Application.Queries.GetListCampaignBasicByEditorId;
using CampaignService.Application.Queries.GetListCampaignByCustomerId;
using CampaignService.Application.Queries.GetListCampaignByEditorId;
using CampaignService.Application.Queries.GetListCampaignByMarketerId;
using CampaignService.Application.Queries.GetListCampaignByUserId;
using CampaignService.Application.Queries.GetListCampaignByWriterId;
using CampaignService.Models;
using CampaignService.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CampaignService.Controllers
{
    public class CampaignController : BaseController
    {
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetListCampaign()
        {
            var response = await Mediator.Send(new GetListCampaignRequest());
            if (response.Count() == 0)
            {
                return BadRequest("Don't have Campaign");
            }
            return Ok(response);
        }

        [HttpGet("campaigns/{id}")]
        [Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetCampaignDetailAsync(int id)
        {
            var response = await Mediator.Send(new GetCampaignRequest { IdCampaign = id });
            return Ok(response);
        }

        [HttpGet("campaigns/customers/{id}")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetListCampaignByUserIdAsync(int id)
        {
            var response = await Mediator.Send(new GetListCampaignByUserIdRequest { IdCustomer = id });
            if (response.Count() == 0)
            {
                return BadRequest("Don't have Campaign for Customer");
            }
            return Ok(response);
        }

        [HttpGet("campaigns/marketers/{id}")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetListCampaignByMarketerIdAsync(int id)
        {
            var response = await Mediator.Send(new GetCampaignByMarketerIdRequest { IdMarketer = id });
            if (response.Count() == 0)
            {
                return BadRequest("Don't have Campaign for Marketer");
            }
            return Ok(response);
        }

        [HttpPost("campaign")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> PostCampaignAsync(CreateCampaignCommand command)
        {
            var response = await Mediator.Send(command);
            //Create exchange
            Producer producer = new Producer();
            CampaignData campaignDTO = new CampaignData();
            campaignDTO = response;
            producer.PublishMessage(message: JsonConvert.SerializeObject(campaignDTO), "CreateCampaign");
            return Accepted(response);
        }

        [HttpPut("campaign")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> PostCampaignAsync(UpdateCampaignCommand command)
        {
            var response = await Mediator.Send(command);
            return Accepted(response);
        }

        [HttpGet("campaigns/editor/{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetListCampaignByEditorIdAsync(int id)
        {
            var response = await Mediator.Send(new GetCampaignByEditorIdRequest { IdEditor = id });
            return Ok(response);
        }

        [HttpGet("campaigns-basic/writer/{id}")]
        ////[Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetListCampaignByWriterIdAsync(int id)
        {
            var response = await Mediator.Send(new GetListCampaignByWriterIdRequest { IdWriter = id });
            return Ok(response);
        }
        [HttpGet("campaigns-basic/editor/{id}")]
        ////[Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetListCampaignBasicByEditorIdAsync(int id)
        {
            var response = await Mediator.Send(new GetListCampaignBasicByEditorIdRequest { IdEditor = id });
            return Ok(response);
        }
        [HttpGet("campaigns-statics/customer/{id}")]
        ////[Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetListCampaignStaticsByCustomerIdAsync(int id)
        {
            var response = await Mediator.Send(new GetListCampaignByCustomerIdRequest { Id = id });
            return Ok(response);
        }
        [HttpPost("campaigns-total-statics/customer")]
        ////[Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetListCampaignTotalStaticsAsync(GetCampaignStaticsRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}