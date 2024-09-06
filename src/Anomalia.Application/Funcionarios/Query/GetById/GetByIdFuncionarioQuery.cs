using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Funcionarios.Query;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Funcionarios.Query.GetById;
public record class GetByIdFuncionarioQuery(FuncionarioId Id) : IQuery<FuncionarioResponse>;
