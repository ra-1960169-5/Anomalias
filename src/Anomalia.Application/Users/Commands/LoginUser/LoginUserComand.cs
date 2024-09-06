using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Users.Commands.LoginUser;

public sealed record LoginUserComand(string Email, string Password) : ICommand;