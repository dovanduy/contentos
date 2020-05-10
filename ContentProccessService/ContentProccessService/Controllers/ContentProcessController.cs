using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentProccessService.Application.Dtos;
using ContentProccessService.Application.Queries.GetTags;
using Microsoft.AspNetCore.Mvc;
using ContentProccessService.Application.Commands.CreateTag;
using ContentProccessService.Application.Commands.CreateTask;
using ContentProccessService.Application.Commands.CreateTasksChannel;
using ContentProccessService.Application.Commands.UpdateTaskChannel;
using ContentProccessService.Application.Queries.GetTagsByCampaignId;
using ContentProccessService.Application.Queries.GetListTaksByCampaignId;
using ContentProccessService.Application.Queries.GetTaskDetail;
using ContentProccessService.Application.Queries.GetListTaskByIdMarketer;
using ContentProccessService.Application.Queries.GetTasksByEditorId;
using ContentProccessService.Models;
using ContentProccessService.Application.Commands.CreateTasks;
using ContentProccessService.Application.Commands.CreateListTaskChannel;
using ContentProccessService.Entities;
using Microsoft.AspNetCore.Authorization;
using ContentProccessService.Application.Commands.ApproveRejectContent;
using ContentProccessService.Application.Commands.UpdatetTaskEditor;
using ContentProccessService.Application.Queries.GetTaskDetailUpdate;
using ContentProccessService.Application.Commands.DeleteTask;
using ContentProccessService.Application.Commands.SaveContent;
using ContentProccessService.Application.Commands.SubmitContent;
using ContentProccessService.Application.Commands.StartTask;
using ContentProccessService.Application.Queries.GetListTaskByIdWriter;
using ContentProccessService.Application.Queries.GetAllListTaskByIdEditor;
using ContentProccessService.Application.Queries.GetContentViewer;
using Microsoft.AspNetCore.Http;
using ContentProccessService.Application.Queries.GetContentDetail;
using ContentProccessService.Application.Queries.GetAllListStatus;
using ContentProccessService.Application.Queries.GetAllStatusCampaign;
using ContentProccessService.Application.Queries.GetStatusPublish;
using AuthenticationService.Application.Queries.GetAds;
using ContentProccessService.Application.Queries.GetTrend;
using ContentProccessService.Application.Commands.CountClickContent;
using ContentProccessService.Application.Queries.GetContentViewerByTag;
using ContentProccessService.Application.Queries.GetContentViewerRecommend;
using ContentProccessService.RabbitMQ;
using Newtonsoft.Json;
using ContentProccessService.Application.Queries.GetStatistics;
using ContentProccessService.Application.Queries.GetStatisticsOneMonth;
using ContentProccessService.Application.Queries.GetStaticsForCustomer;
using ContentProccessService.Application.Queries.GetTaskbyTagInteraction;
using ContentProccessService.Application.Queries.GetTaskViewTrend;
using ContentProccessService.Application.Queries.GetTaskbyTagInteractionMonth;
using ContentProccessService.Application.Queries.GetTaskViewTrendMonth;
using ContentProccessService.Application.Queries.GetTaskByCampaignIdInteraction;

namespace ContentProccessService.Controllers
{
    public class ContentProcessController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContentProcessController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("tags")]
        //[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetListTagAsync()
        {
            var response = await Mediator.Send(new GetTagRequest());
            if (response.Count() == 0)
            {
                return BadRequest("Don't have tags");
            }
            return Ok(response);
        }

        [HttpPost("tags")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTag([FromBody]TagsDto dto)
        {
            var response = await Mediator.Send(new CreateTagRequest { dto = dto });
            return Ok(response);
        }

        [HttpGet("tags/campaign/{id}")]
        [Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetTagsByCampaignId(int id)
        {
            var response = await Mediator.Send(new GetTagsByCampaignIdRequest { CampaignId = id });
            return Ok(response);
        }
        [HttpGet("task/campaign/{id}")]
        [Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetTasksByCampaignId(int id)
        {
            var response = await Mediator.Send(new GetListTasksByCampaignIdRequest { IdCampaign = id });
            return Ok(response);
        }
        [HttpGet("task-detail/campaign/{id}")]
        [Authorize(Roles = "Marketer,Editor,Writer")]
        public async Task<IActionResult> GetTasksDetail(int id)
        {
            var response = await Mediator.Send(new GetTaskDetailRequest { IdTask = id });
            return Ok(response);
        }
        [HttpGet("task/marketer/{id}")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetTasksByMarketerId(int id)
        {
            var response = await Mediator.Send(new GetListTaskByIdMarketerRequest { IdMartketer = id });
            return Ok(response);
        }
        [HttpGet("task/editor/{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetTasksByEditorId(int id)
        {
            var response = await Mediator.Send(new GetTasksByEditorIdRequest { IdEditor = id });
            return Ok(response);
        }
        [HttpPost("taskschannels")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> CreateTaskChannel([FromBody]TaskChannelModel taskchannel)
        {
            var response = await Mediator.Send(new CreateTaskChannelRequest { IdTask = taskchannel.IdTask, IdChannel = taskchannel.IdChannel });
            return Ok(response);
        }

        [HttpDelete("taskschannels/{id}")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> UpdateTaskChannel(int id)
        {
            var response = await Mediator.Send(new UpdateTaskChannelRequest { IdTaskChannel = id });
            return Ok(response);
        }
        [HttpPost("task")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel taskchannel)
        {
            var response = await Mediator.Send(new CreateTaskRequest { Task = taskchannel });
            //Create exchange
            Producer producer = new Producer();
            var TaskDTO = new TasksViewModelMessage()
            {
                Campaign = response.Campaign,
                EmailWriter = response.EmailWriter,
                Title = response.Title
            };
            producer.PublishMessage(message: JsonConvert.SerializeObject(TaskDTO), "CreateTask");
            return Accepted(response);
        }
        [HttpPost("tasks")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> CreateTasks([FromBody] List<CreateTaskModel> taskchannel)
        {
            var response = await Mediator.Send(new CreateTasksRequest { tasks = taskchannel });
            return Ok(response);
        }
        [HttpPost("approvals")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> ApprovalsContent(ApproveRejectContentRequest command)
        {
            var response = await Mediator.Send(command);
            if (response)
            {
                return Accepted("Successful!!");
            }
            return Conflict("Fail!!");
        }
        [HttpPut("task")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> UpdateTaskEditor(UpdateTaskEditorCommand command)
        {
            var response = await Mediator.Send(command);
            if (response == null)
            {
                return Conflict("Update Fail");
            }
            return Accepted(response);
        }
        [HttpGet("task-detail-update/campaign/{id}")]
        [Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetTaskUpdateDetail(int id)
        {
            var response = await Mediator.Send(new GetTaskDetailUpdateRequest { IdTask = id });
            return Ok(response);
        }
        [HttpDelete("task/campaign/{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var response = await Mediator.Send(new DeleteTaskRequest { IdTask = id });
            if (response == true)
            {
                return Ok(response);
            }
            return NoContent();

        }
        [HttpPut("content/task/campaign")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> SaveContent(SaveContentCommand command)
        {
            var response = await Mediator.Send(command);
            if (response == null)
            {
                return Conflict("Save Fail!!");
            }
            return Ok(response);
        }
        [HttpPut("content/task/campaign/submit")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> SubmitContent(SubmitContentCommand command)
        {
            await Mediator.Send(command);
            return Accepted("Successfull!!");
        }
        [HttpPut("content/task/campaign/start")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> StartTask(StartTaskCommand command)
        {
            var response = await Mediator.Send(command);
            return Accepted(response);
        }
        [HttpGet("task/writer/{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetTasksByWriterId(int id)
        {
            var response = await Mediator.Send(new GetListTaskByIdWriterRequest { IdWriter = id });
            return Ok(response);
        }
        [HttpGet("all-task/editor/{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetAllTasksByEditorId(int id)
        {
            var response = await Mediator.Send(new GetAllListTaskByIdEditorRequest { IdEditor = id });
            return Ok(response);
        }
        [HttpPost("content/viewer")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetContent(GetContentViewerRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("cookies/viewer")]
        //[Authorize(Roles = "")]
        public void CreateCookies(string key, string value, int? expireTime)
        {
            //CookieOptions option = new CookieOptions();

            //if (expireTime.HasValue)
            //    option.Expires = DateTime.UtcNow.AddMinutes(expireTime.Value);
            //else
            //    option.Expires = DateTime.UtcNow.AddMilliseconds(10);

            //Response.Cookies.Append(key, value, option);
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                IsEssential = true, //<- there
                Expires = DateTime.UtcNow.AddYears(1),
                SameSite = SameSiteMode.None
            };
            if (string.IsNullOrEmpty(value))
            {
                value = "0";
            }
            if (expireTime.HasValue)
                cookieOptions.Expires = DateTime.UtcNow.AddMonths(expireTime.Value);
            else
                cookieOptions.Expires = DateTime.UtcNow.AddYears(1);
            Response.Cookies.Append(key, value, cookieOptions);
        }
        [HttpGet("content-detail/viewer/{id}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetContentDetail(int id)
        {
            var response = await Mediator.Send(new GetContentDetailRequest { IdTask = id });
            return Ok(response);
        }
        [HttpGet("status/task")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAllStatus()
        {
            var response = await Mediator.Send(new GetAllStatusRequest());
            return Ok(response);
        }
        [HttpGet("status/campaign")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAllStatusCampaign()
        {
            var response = await Mediator.Send(new GetAllStatusCampaignRequest());
            return Ok(response);
        }
        [HttpGet("status-publish/task")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetPublishStatusTask()
        {
            var response = await Mediator.Send(new GetStatusPublishRequest());
            return Ok(response);
        }

        [HttpGet("content/ads")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetAds()
        {
            var response = await Mediator.Send(new GetAdsRequest() { });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }

        [HttpGet("content/trends")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetTrends()
        {
            var response = await Mediator.Send(new GetTrendRequest() { });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpPut("count-content")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CountClickCotent(CountClickContentCommands command)
        {
            await Mediator.Send(command);
            return Accepted("Successfull!!");
        }
        [HttpGet("content/tag/{id}")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetContentByTag(int id)
        {
            var response = await Mediator.Send(new GetContentViewerByTagRequest() { IdTag = id });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }

        [HttpGet("content/recommend/{id}")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetContentRecommend(int id)
        {
            var response = await Mediator.Send(new GetContentViewerRecommendRequest() { Id = id });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-one-week")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetStatisticsOneWeek(int quantity)
        {
            var response = await Mediator.Send(new GetStatisticsRequest() {Quantity = quantity });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-one-month")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetStatisticsOneMonth(int quantity)
        {
            var response = await Mediator.Send(new GetStatisticsOneMonthRequest() { Quantity = quantity });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-for-customer/{id}")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetStatisticsForCustomer(int id)
        {
            var response = await Mediator.Send(new GetStaticsForCustomerRequest {IdCampaign = id });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-by-tag/{id}")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetTaskInteractionByTags(int id)
        {
            var response = await Mediator.Send(new GetTaskbyTagInteractionRequest { Id = id });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpPost("statistics-by-tag-month")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetTaskInteractionByTagsMonth(GetTaskbyTagInteractionMonthRequest request)
        {
            var response = await Mediator.Send(request);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-task-trend")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetTaskInteractionTrend()
        {
            var response = await Mediator.Send(new GetTaskViewTrendRequest { });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-task-trend-month")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetTaskInteractionTrendMonth()
        {
            var response = await Mediator.Send(new GetTaskViewTrendMonthRequest { });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
        [HttpGet("statistics-task-campaignid/{id}")]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> GetTaskInteractionByCampaignId(int id)
        {
            var response = await Mediator.Send(new GetTaskByCampaignIdInteractionRequest { IdCampaign = id });
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Please check fanpage id");
            }

        }
    }
}