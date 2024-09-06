using Anomalias.Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Problemas.Commands.Delete;
internal sealed class DeleteProblemaCommandValidator : AbstractValidator<DeleteProblemaCommand>
{

    public DeleteProblemaCommandValidator(IApplicationDbContext context)
    {
        RuleFor(c => c.Id).MustAsync(async (id, _) =>
        {
            return await context.Problemas.AsNoTracking().AnyAsync(x => x.Id == id, _);
        }).WithMessage("Problema inexistente!");


    }

}
