using Anomalias.Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Setores.Commands.Delete;
internal sealed class DeleteSetorCommandValidator : AbstractValidator<DeleteSetorCommand>
{

    public DeleteSetorCommandValidator(IApplicationDbContext context)
    {
        RuleFor(c => c.Id).MustAsync(async (id, _) =>
        {
            return await context.Setores.AsNoTracking().AnyAsync(x => x.Id == id, _);
        }).WithMessage("Setor inexistente!");


    }

}
