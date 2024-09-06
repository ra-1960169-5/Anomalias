using Anomalias.Application.Abstractions.Data;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Funcionarios.Query;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Funcionarios.Query.GetById;
internal class GetByIdFuncionarioQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetByIdFuncionarioQuery, FuncionarioResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<FuncionarioResponse>> Handle(GetByIdFuncionarioQuery request, CancellationToken cancellationToken)
    {
        var funcionario = await _dbContext.Funcionarios.AsNoTracking().Where(w => w.Id == request.Id).Select(funcionario => funcionario.ToResponse()).FirstOrDefaultAsync(cancellationToken);
        if (funcionario is null) return Result.Failure<FuncionarioResponse>(DomainErrors.FuncionarioErrors.NotFound);
        return Result.Success(funcionario);
    }
}
