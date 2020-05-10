using FluentValidation;


namespace ContentProccessService.Application.Commands.CreateTask
{
    public class CreateTaskValidator:AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Task.Description).NotEmpty().WithMessage("Desciption is not empty");
            RuleFor(x => x.Task.IdCampaign).NotEmpty().WithMessage("Id Campaign is missing");
            RuleFor(x => x.Task.IdWriter).NotEmpty().WithMessage("Id writer is missing");
        }
    }
}
