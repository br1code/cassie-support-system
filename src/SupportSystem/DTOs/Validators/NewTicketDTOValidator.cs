using FluentValidation;

namespace SupportSystem.DTOs.Validators
{
    public class NewTicketDTOValidator : AbstractValidator<NewTicketDTO>
    {
        public NewTicketDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
