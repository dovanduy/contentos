
using System.Threading.Tasks;
using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Commands.ChangePassword;
using AuthenticationService.Application.Commands.CheckOldPassword;
using AuthenticationService.Application.Commands.CreateCustomer;
using AuthenticationService.Application.Commands.CreateUser;
using AuthenticationService.Application.Commands.DeleteUser;
using AuthenticationService.Application.Commands.DisableAccount;
using AuthenticationService.Application.Commands.Notify;
using AuthenticationService.Application.Commands.SaveToken;
using AuthenticationService.Application.Commands.UpdateCustomer;
using AuthenticationService.Application.Commands.UpdateProfile;
using AuthenticationService.Application.Commands.UpdateUser;
using AuthenticationService.Application.Queries;
using AuthenticationService.Application.Queries.AuthenticationViewer;
using AuthenticationService.Application.Queries.CheckEmail;
using AuthenticationService.Application.Queries.GetAllWriterByIdMarketer;
using AuthenticationService.Application.Queries.GetCustomer;
using AuthenticationService.Application.Queries.GetCustomerByIdEditor;
using AuthenticationService.Application.Queries.GetcustomerDetail;
using AuthenticationService.Application.Queries.GetListAccountFreeStatus;
using AuthenticationService.Application.Queries.GetListEditorBasic;
using AuthenticationService.Application.Queries.GetListEditorForWriter;
using AuthenticationService.Application.Queries.GetListMarketerBasic;
using AuthenticationService.Application.Queries.GetListUser;
using AuthenticationService.Application.Queries.GetListViewer;
using AuthenticationService.Application.Queries.GetListWriterBasic;
using AuthenticationService.Application.Queries.GetManagerByUserId;
using AuthenticationService.Application.Queries.GetNotify;
using AuthenticationService.Application.Queries.GetProfile;
using AuthenticationService.Application.Queries.GetUser;
using AuthenticationService.Application.Queries.GetUserDetail;
using AuthenticationService.Application.Queries.GetWriter;
using AuthenticationService.Entities;
using AuthenticationService.Models;
using AuthenticationService.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenticationService.Controllers
{
    public class AuthenticationController : BaseController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticationRequest queries)
        {
            var response = await Mediator.Send(queries);
            if (response == null)
            {
                return BadRequest("Invalid User Name of Password");
            }
            if (response.IdError == 1)
            {
                return Forbid();
            }
            return Ok(response);
        }
        [HttpPost("Login-Viewer")]
        public async Task<IActionResult> LoginViewer(AuthenticationViewerRequest queries)
        {
            var response = await Mediator.Send(queries);
            if (response == null)
            {
                return BadRequest("Invalid User Name of Password");
            }
            if (response.IdError == 1)
            {
                return Forbid();
            }
            return Ok(response);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommands commands)
        {
            var response = await Mediator.Send(commands);
            if (response == false)
            {
                return BadRequest("Change Password Fail");
            }
            return Accepted("Change Success ");
        }
        [HttpPost("check-password")]
        public async Task<IActionResult> CheckPassword(CheckOldPasswordCommands commands)
        {
            var response = await Mediator.Send(commands);
            if (response == false)
            {
                return BadRequest("Password Was Wrong");
            }
            return Accepted("Right Password");
        }
        [HttpPost("Register")]
        ////[Authorize(Roles = "Guest")]
        public async Task<object> Register(RegisterAccountCommands command)
        {
           var response = await Mediator.Send(command);
            if (response == 1)
            {
                return Accepted("Create Successful !!");
            }
            if (response == 0)
            {
                return Conflict("Email is exsit !!");
            }

            return BadRequest("Create Fail !!");
        }
        [HttpGet("Editors/Marketers/{id}")]
        [Authorize(Roles = "Marketer,Editor,Admin")]
        public async Task<IActionResult> GetListEditor(int id)
        {
            var response = await Mediator.Send(new GetUserRequest { IdMarketer = id });
            if (response.Count == 0)
            {
                return BadRequest("Don't have Editor For Marketer");
            }
            return Ok(response);

        }

        [HttpGet("Writers/Editors/{id}")]
        [Authorize(Roles = "Marketer,Editor,Admin")]
        public async Task<IActionResult> GetListWriter(int id)
        {
            var response = await Mediator.Send(new GetWriterRequest { EditorId = id });
            if (response.Count == 0)
            {
                return BadRequest("Don't have Writter For Marketer");
            }
            return Ok(response);
        }

        [HttpGet("Customers/Marketers-Basic/{id}/")]
        [Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListCustomerBasic(int id)
        {
            var response = await Mediator.Send(new GetCustomerBasicRequest { MarketerId = id });
            return Ok(response);
        }

        [HttpGet("Customers/Marketers/{id}")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetListCustomer(int id)
        {
            var response = await Mediator.Send(new GetCustomerRequest { MarketerId = id });
            if (response.Count == 0)
            {
                return BadRequest("Don't have Customer For Marketer");
            }
            return Ok(response);
        }

        [HttpPost("Customers")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> CreateCustomerAccounts(CreateCustomerAccountCommads command)
        {
            var result = await Mediator.Send(command);
            //Create exchange
            //Producer producer = new Producer();
            //MessageAccountDTO messageDTO = new MessageAccountDTO{
            //   FullName = result.FullName,Password = result.Password,Email = result.Email };
            //producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");]
            if (result == null)
            {
                return BadRequest("Create Fail !!");
            }
            if (result.IdError == 0)
            {
                return Conflict("Duplicate Email !!");
            }
            return Accepted(result);

        }
        [HttpPut("Customers")]
        [Authorize(Roles = "Marketer")]
        public async Task<IActionResult> UpdateCustomerAccounts(UpdateCustomerAccountCommads command)
        {
            var result = await Mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Update fail");
            }
            return Accepted(result);
        }
        [HttpPost("send-notify")]
        public async Task<IActionResult> Notify(NotifyCommands Command)
        {
            var response = await Mediator.Send(Command);
            if (response == null)
            {
                return BadRequest("Something went wrong, Please check recieverId and token");
            }
            return Ok(response);
        }
        [HttpGet("Writer/Marketers/{id}")]
        ////[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetListWriterByIdMarketer(int id)
        {
            var response = await Mediator.Send(new GetAllWriterByIdMarketerRequest { MarketerId = id });
            if (response.Count == 0)
            {
                return BadRequest("Don't have Editor For Marketer");
            }
            return Ok(response);
        }
        [HttpGet("customer/editor/{id}")]
        ////[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetcustomerByIdEditor(int id)
        {
            var response = await Mediator.Send(new GetCustomerByIdEditorRequest { EditorId = id });
            if (response.Count == 0)
            {
                return BadRequest("Don't have Editor For Marketer");
            }
            return Ok(response);

        }
        [HttpGet("customer-detail/{id}")]
        ////[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetcustomerDetail(int id)
        {
            var response = await Mediator.Send(new GetCustomerDetailRequest { IdCustomer = id });
            if (response == null)
            {
                return BadRequest("Don't have Account");
            }
            return Ok(response);

        }
        [HttpGet("profile/{id}")]
        ////[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> GetProfileUser(int id)
        {
            var response = await Mediator.Send(new GetProfileRequest { IdUser = id });
            if (response == null)
            {
                return BadRequest("Don't have Account");
            }
            return Ok(response);

        }
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileUser(UpdateProfileCommands command)
        {
            var result = await Mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Create Fail");
            }
            return Accepted(result);
        }
        [HttpPost("Tokens")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> SaveToken(SaveTokenCommands command)
        {
            var result = await Mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Token exits already");
            }
            return Accepted(result);
        }

        [HttpGet("Notify/User/{id}")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListNotifyByUser(int id)
        {
            var response = await Mediator.Send(new GetNotifysRequest { UserId = id });
            if (response.Count == 0)
            {
                return BadRequest("Don't have Notify For Marketer");
            }
            return Ok(response);
        }
        [HttpGet("user/admin")]
        public async Task<IActionResult> GetListUserAdmin()
        {
            var result = await Mediator.Send(new GetListUserRequest { });
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Accepted(result);
        }
        [HttpGet("viewer/admin")]
        public async Task<IActionResult> GetListViewerAdmin()
        {
            var result = await Mediator.Send(new GetListViewerRequest { });
            if (result.Count == 0)
            {
                return NoContent();
            }
            return Accepted(result);
        }
        [HttpPost("user")]
        //[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> CreateUserAccounts(CreateUserCommands command)
        {
            var result = await Mediator.Send(command);

            if (result == null)
            {
                return BadRequest("Create Fail !!");
            }
            if (result.IdError == 0)
            {
                return Conflict("Duplicate Email !!");
            }
            //Create exchange
            Producer producer = new Producer();
            MessageAccountDTO messageDTO = new MessageAccountDTO
            {
                FullName = result.FullName,
                Password = result.Password,
                Email = result.Email
            };
            producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            result.Password = null;
            return Accepted(result);

        }
        [HttpGet("Marketers-Basic")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListMarketerBasic()
        {
            var response = await Mediator.Send(new GetListMarketerBasicRequest { });
            if (response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);
        }
        [HttpGet("Writer-Basic")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListWriterBasic()
        {
            var response = await Mediator.Send(new GetListWriterBasicRequest { });
            if (response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);
        }
        [HttpGet("Editor-Basic")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListEditorBasic()
        {
            var response = await Mediator.Send(new GetListEditorBasicRequest { });
            if (response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);
        }
        [HttpPut("user-delete")]
        //[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> DeleteUserAccounts(DeleteUserCommands command)
        {
            var result = await Mediator.Send(command);

            if (result == null)
            {
                return BadRequest("User doesn't exit !!");
            }
            //Create exchange
            //Producer producer = new Producer();
            //MessageAccountDTO messageDTO = new MessageAccountDTO
            //{
            //    FullName = result.FullName,
            //    Password = result.Password,
            //    Email = result.Email
            //};
            //producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            //result.Password = null;
            return Accepted(result);

        }
        [HttpGet("User-Detail/{id}")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            var response = await Mediator.Send(new GetUserDetailRequest { Id = id });
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
        [HttpPut("user")]
        //[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> UpdateUserAccounts(UpdateUserCommands command)
        {
            var result = await Mediator.Send(command);

            if (result == null)
            {
                return BadRequest("Create Fail !!");
            }
            //Create exchange
            //Producer producer = new Producer();
            //MessageAccountDTO messageDTO = new MessageAccountDTO
            //{
            //    FullName = result.FullName,
            //    Password = result.Password,
            //    Email = result.Email
            //};
            //producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            //result.Password = null;
            return Accepted(result);

        }
        [HttpGet("Editor-Basic-Writer")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListEditorWriterBasic()
        {
            var response = await Mediator.Send(new GetListEditorForWriterRequest { });
            if (response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);
        }


        [HttpGet("Free-User")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetListUserFree()
        {
            var response = await Mediator.Send(new GetListAccountFreeStatusRequest { });
            if (response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);
        }
        [HttpPut("accounts")]
        //[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> DisableAccount(DisableAccountCommand command)
        {
            var result = await Mediator.Send(command);

            if (!result.Equals("Success"))
            {
                return BadRequest();
            }
            
            return Accepted();

        }
        [HttpPost("email")]
        //[Authorize(Roles = "Marketer")]
        public async Task<IActionResult> CheckEmail(CheckEmailRequest request)
        {
            var response = await Mediator.Send(request);
            if (response == false)
            {
                return BadRequest("Email is Existed");
            }
            return Ok("Right Email");
        }
        [HttpGet("Manager-Id/{id}")]
        //[Authorize(Roles = "Marketer,Editor")]
        public async Task<IActionResult> GetManagerAccount(int id)
        {
            var response = await Mediator.Send(new GetManagerByUserIdRequest { id = id });
            return Ok(response);

        }
    }
}
