using ContentProccessService.Entities;
using MediatR;


namespace ContentProccessService.Application.Commands.UpdateTaskChannel
{
    public class UpdateTaskChannelRequest : IRequest<TasksFanpages>
    {
        public int IdTaskChannel { get; set; }
    }
}
