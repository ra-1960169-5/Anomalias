using FluentValidation;

namespace Anomalias.Application.Users.Commands.ResetPasswordUser;
public sealed class ResetPasswordUserCommandValidator: AbstractValidator<ResetPasswordUserCommand>      
{
    public ResetPasswordUserCommandValidator()
    {
        RuleFor(p => p.Password)
                .NotEmpty().WithMessage("A Senha é Obrigatório!")
                .Length(6, 15).WithMessage("O campo senha precisar ter entre 6 e 15 caracteres!")
                .MinimumLength(6).WithMessage("O comprimento da sua senha deve ser pelo menos 6!")
                .MaximumLength(15).WithMessage("O comprimento da sua senha não deve exceder 15!")
                .Matches(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)).+$").WithMessage("A Senha deve conter letras maiúscula, minúscula, numero e caracter especial");


        RuleFor(c => c.Email)
       .NotEmpty().WithMessage("O Email é Obrigatório!")
       .EmailAddress().WithMessage("É necessário um endereço de e-mail válido!");


        RuleFor(c => c.Code).NotEmpty().WithMessage("O CODE é Obrigatório!");
    }
}

