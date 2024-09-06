using Anomalias.Application.Abstractions.Data;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Commands.RegisterAnomalia;
public sealed class RegisterAnomaliaCommandValidator : AbstractValidator<RegisterAnomaliaCommand>
{
    public RegisterAnomaliaCommandValidator(IApplicationDbContext context)
    {
        RuleFor(c => c.UsuarioAberturaId).MustAsync(async (c, _) =>
        {
            return await context.Funcionarios.AsNoTracking().Where(x => x.Id == FuncionarioId.Create(c)).AnyAsync(_);
        }).WithMessage(DomainErrors.FuncionarioErrors.NotFound.Description).DependentRules(() =>
        {

            RuleFor(c => c).MustAsync(async (c, _) =>
            {
                return await context.Funcionarios.AsNoTracking().Where(x => x.Id == FuncionarioId.Create(c.UsuarioAberturaId) && x.SetorId != SetorId.TryParse(c.SetorId)).AnyAsync(_);
            }).WithMessage(DomainErrors.AnomaliaErrors.AddToYour.Description);

        });


    }
}
