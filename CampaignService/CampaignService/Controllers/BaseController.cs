using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CampaignService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class BaseController : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator => mediator ?? (mediator = HttpContext.RequestServices.GetRequiredService<IMediator>());
    }
}