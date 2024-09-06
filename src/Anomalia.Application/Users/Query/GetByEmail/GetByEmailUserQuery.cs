using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Users.Query;

namespace Anomalias.Application.Users.Query.GetByEmail;
public sealed record GetByEmailUserQuery(string Email) : IQuery<UserResponse>;

