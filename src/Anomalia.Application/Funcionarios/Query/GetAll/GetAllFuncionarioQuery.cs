using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Funcionarios.Query;

namespace Anomalias.Application.Funcionarios.Query.GetAll;
public record GetAllFuncionarioQuery : IQuery<List<FuncionarioResponse>>;

