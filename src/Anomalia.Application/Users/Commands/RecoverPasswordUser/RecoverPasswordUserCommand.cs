using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Users.Commands.RecoverPasswordUser;
public sealed record RecoverPasswordUserCommand(string Email):ICommand;
