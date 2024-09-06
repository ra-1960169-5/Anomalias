using Anomalias.Domain.Entities;
using Anomalias.Shared;

namespace Anomalias.Domain.Errors;
public static class DomainErrors
{
    public static class CargoErrors
    {
        public static readonly Error NotFound = new(
            "Cargo.NotFound",
            "Cargo não encontrado!");

        public static readonly Error Add = new(
            "Cargo.Add",
            "Falha ao Cadastrar!");

        public static readonly Error Delete = new(
           "Cargo.Delete",
           "Falha ao Deletar!");

        public static readonly Error Create = new(
          "Cargo.Create",
          "Falha ao Criar o Cargo!");
    }
    public static class ProblemaErrors
    {
        public static readonly Error NotFound = new(
            "Problema.NotFound",
            "Problema não encontrado!");

        public static readonly Error Add = new(
            "Problema.Add",
            "Falha ao Cadastrar!");

        public static readonly Error Delete = new(
           "Problema.Delete",
           "Falha ao Deletar!");


        public static readonly Error Create = new(
         "Problema.Create",
         "Falha ao Criar o Problema!");
    }
    public static class SetorErrors
    {
        public static readonly Error NotFound = new(
            "Setor.NotFound",
            "Setor não encontrado!");

        public static readonly Error Add = new(
            "Setor.Add",
            "Falha ao Cadastrar!");

        public static readonly Error Delete = new(
           "Setor.Delete",
           "Falha ao Deletar!");

        public static readonly Error Create = new(
              "Setor.Create",
              "Falha ao Criar o Setor!");

        public static readonly Error Update = new(
            "Setor.Update",
            "Falha ao Atualizar o Setor!");
    }
    public static class FuncionarioErrors
    {
        public static readonly Error Create = new(
           "Funcionario.Create",
           "Falha ao Criar o Funcionario!");

        public static readonly Error Add = new(
              "Funcionario.Add",
              "Falha ao Cadastrar!");

        public static readonly Error Update = new(
           "Funcionario.Update",
           "Falha ao Atualiazar!");

        public static readonly Error NotFound = new(
          "Funcionario.NotFound",
          "Funcionario não encontrado!");

    }
    public static class AnomaliaErrors
    {
        public static readonly Error AddToYour = new(
        "Anomalia.AddToYour",
        "Não é possivel registrar uma anomalia para o seu proprio setor!");

        public static readonly Error Add = new(
        "Anomalia.Add",
        "Falha ao Cadastrar!");

        public static readonly Error NotFound = new(
        "Anomalia.NotFound",
        "Anomalia não encontrada!");
        public static readonly Error AddComentario = new(
        "Anomalia.AddComentario",
        "Falha ao cadastrar comentario!"
        );
        public static readonly Error EndAnomalia = new(
        "Anomalia.EndAnomalia",
        "Falha ao encerrar a anomalia!"
        );

        public static readonly Error AddUserEnd = new(
           "Anomalia.AddUserEnd",
           "Falha ao adcionar o usuario de Encerramento!"
        );

        public static readonly Error AddEndDate = new(
        "Anomalia.AddEndDate",
        "Falha ao adcionar o data de Encerramento!"
        );

        public static readonly Error UpdateStatus = new(
        "Anomalia.UpdateStatus",
        "Falha ao Atualizar o Status de Encerramento!"
        );

        public static readonly Error AddFinalThoughts = new(
        "Anomalia.AddFinalThoughts",
        "Falha ao Adicionar as Considerações Finais de Encerramento!"
        );

        public static readonly Error CannotEnd = new(
        "Anomalia.CannotEnd",
        "Falha ao Encerrar anomalia!"
         );

        public static readonly Error Create = new(
        "Anomalia.Create",
        "Falha ao Criar a Anomalia!");

        public static Error NotFoundByID(AnomaliaId Id) => new("Anomalia.NotFound",
          $"Anomalia {Id} não encontrada!");

    }
    public static class AnexoErrors
    {
        public static readonly Error NotFound = new(
            "Anexo.NotFound",
            "Anexo não encontrado!");
        public static readonly Error AddAnexo = new(
            "Anexo.Add",
            "Falha ao cadastrar Anexo!"
            );

        public static readonly Error Create = new(
          "Anexo.Create",
         "Falha ao Criar o Anexo!");
    }
    public static class ComentarioErrors
    {

        public static readonly Error AddUserComment = new("Comentario.AddUserComment", "não foi possivel adcionar o usuario do comentario!");

        public static readonly Error AddComment = new("Comentario.AddComment", "não foi possivel adicionar as descrição do comentario!");

        public static readonly Error Create = new(
            "Comentario.Create",
            "Falha ao Criar o Comentario!");
    }
    public static class EmailErrors
    {
        public static readonly Error Empty = new("Email.Empty", "Email Vazio");

        public static readonly Error TooLong = new("Email.TooLong", "Email com tamanho superior ao permitido!");

        public static readonly Error InvalidFormat = new("Email.InvalidFormat", "Email invalido");

        public static readonly Error EmailNotUnique = new("Email.EmailNotUnique", "O e-mail fornecido não é unico!");
    }

}
