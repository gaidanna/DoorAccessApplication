using DoorAccessApplication.Api.Models;
using FluentValidation;

namespace DoorAccessApplication.Api.Validators
{
    public class UpdateLockValidator : AbstractValidator<UpdateLockRequest>
    {
        public UpdateLockValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                        .WithMessage("Id is required.");
            RuleFor(x => x.UniqueIdentifier).NotEmpty()
                                .WithMessage("Unique Identifier is required.");
        }
    }
}
