using Anomalia.Shared;

namespace Anomalia.Identity.Errors;
public static class IdentityErrors
{
    public static class User
    {
        public static readonly Error DuplicateUserName = new("User.DuplicateUserName","Usuario já cadastrador existe!");

        public static readonly Error UserCreate = new("User.UserCreate","Erro ao cadastrar usuarío!");

        public static readonly Error NotFound = new("Users.NotFound", "Usuário não encontrado!");

        public static readonly Error IsLockedOut = new("Users.IsLockedOut", "Usuário Bloqueado!");

        public static readonly Error IsNotAllowed = new("Users.IsNotAllowed", "Usuário Desabilidado!");

    }
}
