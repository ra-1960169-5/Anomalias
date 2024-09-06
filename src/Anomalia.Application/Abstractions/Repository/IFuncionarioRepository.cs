using Anomalias.Domain.Entities;


namespace Anomalias.Application.Abstractions.Repository;

public interface IFuncionarioRepository : IRepository<Domain.Entities.Funcionario, FuncionarioId>
{   
}
