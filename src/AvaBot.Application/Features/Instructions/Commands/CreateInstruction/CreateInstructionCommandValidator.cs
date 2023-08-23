using AvaBot.Domain.Common;
using FluentValidation;

namespace AvaBot.Application.Features.Instructions.Commands.CreateInstruction;
public class CreateInstructionCommandValidator : AbstractValidator<CreateInstructionCommand>
{
    public CreateInstructionCommandValidator()
    {
        RuleFor(x => x.Input)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Common.EmptyField.Code)
            .WithMessage(ErrorCodes.Common.EmptyField.Message);
    }
}
