using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Users.Commands.ResetPasswordUser;

public sealed record ResetPasswordUserCommand(string Email,string Code, string Password) : ICommand;