using DoorAccessApplication.Api.Models;
using FluentValidation;

namespace DoorAccessApplication.Api.Validators
{
    public class CreateLockValidator : AbstractValidator<CreateLockRequest>
    {
        public CreateLockValidator()
        {
            RuleFor(x => x.UniqueIdentifier).NotEmpty()
                                .WithMessage("Unique Identifier is required.");
        }
    }
}
