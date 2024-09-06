using Anomalias.Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Setores.Commands.Create;
public sealed class CreateSetorCommandValidator : AbstractValidator<CreateSetorCommad>
{
    public CreateSetorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(c => c.Descricao).NotNull().WithMessage("A {PropertyName} é Obrigatório!");

        RuleFor(c => c.Descricao).NotEmpty().WithMessage("A {PropertyName} é Obrigatório!");

        RuleFor(c => c.Descricao).Length(2, 50).WithMessage("A {PropertyName} deve ter entre 2 e 50 caracteres");

        RuleFor(c => c.Descricao).MustAsync(
            async (descricao, _) =>
            {
                if (string.IsNullOrEmpty(descricao)) return true;
                return !await context.Setores.AsNoTracking().AnyAsync(prop => prop.Descricao.Equals(descricao.Trim().ToUpper()), _);
            }).WithMessage("Já existe um setor cadastrado com a descricao {PropertyValue}!");

    }
}

