using Anomalias.Shared;

namespace Anomalias.Infrastructure.Identity.Errors;
public static class IdentityErrors
{
    public static class User
    {
        public static readonly Error DuplicateUserName = new("User.DuplicateUserName", "Usuario já cadastrador existe!");

        public static readonly Error UserCreate = new("User.UserCreate", "Erro ao cadastrar usuarío!");

        public static readonly Error NotFound = new("Users.NotFound", "Usuário não encontrado!");

        public static readonly Error IsLockedOut = new("Users.IsLockedOut", "Usuário Bloqueado!");

        public static readonly Error IsNotAllowed = new("Users.IsNotAllowed", "Usuário Desabilidado!");

    }

    public static class Role
    {

        public static readonly Error RoleCreate = new("Role.RoleCreate", "Erro ao cadastrar role!");

        public static readonly Error NotFound = new("Role.NotFound", "role não encontrada!");

        public static readonly Error AddRoleToUser = new("Role.AddRoleToUser", "erro ao adcionar role ao usuario!");
    }
}
