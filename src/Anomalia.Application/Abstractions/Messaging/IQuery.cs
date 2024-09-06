using Anomalias.Shared;
using MediatR;

namespace Anomalias.Application.Abstractions.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
