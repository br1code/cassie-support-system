using FluentValidation;

namespace SupportSystem.DTOs.Validators
{
    public class NewCommentDTOValidator : AbstractValidator<NewCommentDTO>
    {
        public NewCommentDTOValidator() 
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
