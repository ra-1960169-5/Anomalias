using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Users.Commands.DeleteUser;
public sealed record DeleteUserCommand(string Id) : ICommand;
