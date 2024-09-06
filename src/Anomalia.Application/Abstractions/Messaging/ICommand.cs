using Anomalias.Shared;
using MediatR;

namespace Anomalias.Application.Abstractions.Messaging;
public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
