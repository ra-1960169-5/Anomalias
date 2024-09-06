using FluentValidation;

namespace Anomalias.Application.Funcionarios.Commands.CreateFuncionario;
public sealed class CreateFuncionarioCommandValidator : AbstractValidator<CreateFuncionarioCommand>
{
    public CreateFuncionarioCommandValidator()
    {

        RuleFor(c => c.Nome).NotEmpty().WithMessage("O Nome é Obrigatório!");

        RuleFor(c => c.Nome)
        .Length(2, 50).WithMessage("O Nome deve ter entre 2 e 50 caracteres");

        RuleFor(c => c.Setor).NotEmpty().WithMessage("O Setor é Obrigatório!");

        RuleFor(c => c.Cargo).NotEmpty().WithMessage("O Cargo é Obrigatório!");

        RuleFor(c => c.Email)
       .NotEmpty().WithMessage("O Email é Obrigatório!")
       .EmailAddress().WithMessage("É necessário um endereço de e-mail válido!");

    }


}
