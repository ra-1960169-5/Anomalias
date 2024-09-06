using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Users.Commands.CreateUser;
public sealed record CreateUserCommand(string Email, string Password) : ICommand<string>;
