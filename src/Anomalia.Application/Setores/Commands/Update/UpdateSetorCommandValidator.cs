using Anomalias.Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Setores.Commands.Update;
internal sealed class UpdateSetorCommandValidator : AbstractValidator<UpdateSetorCommand>
{

    public UpdateSetorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(c => c.Descricao).NotEmpty().WithMessage("A Descrição é Obrigatório!");

        RuleFor(c => c.Descricao)
        .Length(2, 50).WithMessage("A Descrição deve ter entre 2 e 50 caracteres");

        RuleFor(c => c).MustAsync(
          async (c, _) =>
          {
              if (string.IsNullOrEmpty(c.Descricao)) return true;
              return !await context.Setores.AsNoTracking().AnyAsync(prop => prop.Descricao.Equals(c.Descricao.Trim().ToUpper()) && prop.Id != c.Id, _);
          }).WithMessage("Já existe um setor cadastrado com a descricao!");

    }
}
