using Anomalias.Application.Abstractions.Data;
using Anomalias.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Commands.AddComentario;
internal class AddComentarioCommandValidator : AbstractValidator<AddComentarioCommand>
{
    public AddComentarioCommandValidator(IApplicationDbContext context)
    {
        RuleFor(c => c.Comentario).NotEmpty().WithMessage("A descrição do comentario é Obrigatório!");

        RuleFor(c => c.AnomaliaId).MustAsync(async (anomaliaId, _) =>
        {
            return await context.Anomalias.AsNoTracking().Where(x => x.Id == AnomaliaId.TryParse(anomaliaId)).AnyAsync(_);
        }).WithMessage("não foi possivel registrar o comentario!");


    }
}
