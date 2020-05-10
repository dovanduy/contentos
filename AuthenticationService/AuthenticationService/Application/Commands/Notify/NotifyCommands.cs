using AuthenticationService.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.Commands.Notify
{
    public class NotifyCommands : IRequest<NotifyModel>
    {
     
        [Required]
        public string Sender { get; set; }

        [Required]
        public int Reciever { get; set; }

        [Required]
        public int IdObject { get; set; }

        [Required]
        public string TypeSend { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Messenger { get; set; }


    }
}
