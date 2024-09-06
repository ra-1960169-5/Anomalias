using FluentValidation;

namespace Anomalias.Application.Users.Commands.RecoverPasswordUser;
public sealed class RecoverPasswordUserCommandValidator: AbstractValidator<RecoverPasswordUserCommand>
{
    public RecoverPasswordUserCommandValidator()
    {
      RuleFor(c => c.Email)
     .NotEmpty().WithMessage("O Email é Obrigatório!")
     .EmailAddress().WithMessage("É necessário um endereço de e-mail válido!");

    }
}
