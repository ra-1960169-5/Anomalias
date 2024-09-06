using Anomalias.Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Cargos.Commands.DeleteCargo;
internal class DeleteCargoCommandValidation : AbstractValidator<DeleteCargoCommand>
{
    public DeleteCargoCommandValidation(IApplicationDbContext context)
    {
        RuleFor(c => c.Id).MustAsync(async (id, _) =>
        {
            return await context.Cargos.AsNoTracking().AnyAsync(x => x.Id == id, _);
        }).WithMessage("Cargo inexistente!");


    }
}
