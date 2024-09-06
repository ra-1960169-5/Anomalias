using Anomalias.Application.Abstractions.Data;
using Anomalias.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Commands.EndAnomalia;
internal class EndAnomaliaCommandValidator : AbstractValidator<EndAnomaliaCommand>
{
    public EndAnomaliaCommandValidator(IApplicationDbContext context)
    {

        RuleFor(c => c.ConsideracoesFinais).NotEmpty().WithMessage("As considerações finais é Obrigatório!");

        RuleFor(c => c.AnomalaiId).MustAsync(async (anomaliaId, _) =>
        {
            return await context.Anomalias.AsNoTracking().AnyAsync(x => x.Id == AnomaliaId.TryParse(anomaliaId), _);
        }).WithMessage("não foi possivel encerrar a anomalia!");
    }
}
