using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Funcionarios.Query.GetAll;
internal sealed class GetAllFuncionarioQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetAllFuncionarioQuery, List<FuncionarioResponse>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<List<FuncionarioResponse>>> Handle(GetAllFuncionarioQuery request, CancellationToken cancellationToken)
    {
        var funcionarios = await _dbContext.Funcionarios.AsNoTracking().Where(x => x.Ativo == true).Select(funcionario => funcionario.ToResponse()).ToListAsync(cancellationToken);
        if (funcionarios is null) return Result.Failure<List<FuncionarioResponse>>(DomainErrors.FuncionarioErrors.NotFound);
        return Result.Success(funcionarios);
    }
}
