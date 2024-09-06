using Anomalias.Application.Anexos.Query;
using Anomalias.Application.Anomalias.Query;
using Anomalias.Application.Cargos.Query;
using Anomalias.Application.Funcionarios.Query;
using Anomalias.Application.Problemas.Query;
using Anomalias.Application.Setores.Query;
using Anomalias.Application.ViewModels;

using Anomalias.Domain.Entities;

namespace Anomalias.Application.Extensions;

public static class MappingExtensions
{
    //--Response para ViewModel--//
    public static AnomaliaVM ToViewModel(this AnomaliaResponse anomalia)
    {

        return new(anomalia.Id,
                   anomalia.NumeroRegistro,
                   anomalia.Setor,
                   anomalia.ResponsavelSetor,
                   anomalia.Problema,
                   anomalia.Questionamento,
                   anomalia.ResultadoEsperado,
                   anomalia.ConsideracoesFinais,
                   anomalia.DataAbertura,
                   anomalia.DataEncerramento,
                   anomalia.UsuarioAbertura,
                   anomalia.UsuarioEncerramento,
                   anomalia.Status,
                   anomalia.Restrita,
                   anomalia.AnexoId,
                   [.. anomalia.Comentarios.Select(x => x.ToViewModel())]
        );

    }
    public static ComentarioVM ToViewModel(this ComentarioResponse comentario)
    {
        return new(comentario.Id, comentario.Descricao, comentario.AnexoId, comentario.Data, comentario.ComentadoPor);
    }
    public static CargoVM ToViewModel(this CargoResponse cargo)
    {
        return new(cargo.Id.ToString(), cargo.Descricao);
    }
    public static SetorVM ToViewModel(this SetorResponse setor)
    {
        return new(setor.Id, setor.Descricao, setor.GestorNome, setor.GestorId);
    }
    public static ProblemaVM ToViewModel(this ProblemaResponse problema)
    {
        return new(problema.Id, problema.Descricao);
    }
    public static FuncionarioVM ToViewModel(this FuncionarioResponse funcionario)
    {
        return new(funcionario.Id, funcionario.Nome, funcionario.Setor, funcionario.Cargo, funcionario.Ativo, funcionario.Gestor);
    }
    //--Domain para Reponse--//
    public static AnomaliaResponse ToResponse(this Domain.Entities.Anomalia anomalia)
    {

        return new(anomalia.Id.ToString(),
                   anomalia.Numero,
                   anomalia.Setor?.Descricao,             
                   anomalia.Setor?.Gestor?.Nome ?? "SEM RESPONSÁVEL",
                   anomalia.Problema?.Descricao,
                   anomalia.Questionamento,
                   anomalia.ResultadoEsperado,
                   anomalia.ConsideracoesFinais,
                   anomalia.DataDeAbertura,
                   anomalia.DataDeEncerramento,
                   anomalia.FuncionarioAbertura!.Nome,
                   anomalia.FuncionarioEncerramento?.Nome,
                   (int)anomalia.Status,
                   anomalia.Restrita,
                   anomalia.AnexoAnomaliaId.ToString(),
                   [.. anomalia.Comentarios.ToComentariosReponse()]
        );

    }
    public static ComentarioResponse ToResponse(this Comentario comentario)
    {
        return new(comentario.Id.ToString(), comentario.Descricao, comentario.AnexoComentarioId.ToString(), comentario.DataDoComentario, comentario.Comentador!.Nome);
    }
    public static CargoResponse ToResponse(this Domain.Entities.Cargo cargo)
    {
        return new(cargo.Id.ToString(), cargo.Descricao);
    }
    public static SetorResponse ToResponse(this Domain.Entities.Setor setor)
    {
    
        return new(setor.Id.ToString(), setor.Descricao, setor.Gestor?.Nome ?? "SEM RESPONSÁVEL", setor.Gestor?.Id.ToString());
    }
    public static ProblemaResponse ToResponse(this Domain.Entities.Problema problema)
    {
        return new(problema.Id.ToString(), problema.Descricao);
    }
    public static FuncionarioResponse ToResponse(this Domain.Entities.Funcionario funcionario)
    {      
        return new(funcionario.Id.ToString(), funcionario.Nome, funcionario.SetorId.ToString(), funcionario.CargoId.ToString(), funcionario.Ativo, funcionario.PossuiGestor());
    }
    public static AnexoResponse ToResponse(this Domain.Entities.Anexo anexo)
    {
        return new(anexo.ContentType, anexo.Nome,anexo.Dados);
    }

    //-- IQueryableDomain para IQueryableResponse--//
    public static IQueryable<ComentarioResponse> ToComentariosReponse(this IReadOnlyCollection<Comentario> comentarios)
    {
        return comentarios.Select(comentario => comentario.ToResponse()).AsQueryable();
    }
    public static IQueryable<AnomaliaResponse> ToResponse(this IQueryable<Domain.Entities.Anomalia> anomalias)
    {
        return anomalias.Select(anomalia => anomalia.ToResponse());
    }
    public static IQueryable<CargoResponse> ToResponse(this IQueryable<Domain.Entities.Cargo> cargos)
    {
        return cargos.Select(cargo => cargo.ToResponse()).AsQueryable();
    }
    public static IQueryable<ProblemaResponse> ToResponse(this IQueryable<Domain.Entities.Problema> problemas)
    {
        return problemas.Select(problema => problema.ToResponse()).AsQueryable();
    }
    public static IQueryable<FuncionarioResponse> ToResponse(this IQueryable<Domain.Entities.Funcionario> funcionarios)
    {
        return funcionarios.Select(funcionario => funcionario.ToResponse()).AsQueryable();
    }
    //-- IQueryableResponse para IQueryableViewModel--//
    public static IQueryable<CargoVM> ToViewModel(this IQueryable<CargoResponse> cargos)
    {
        return cargos.Select(cargo => cargo.ToViewModel()).AsQueryable();
    }
    public static IQueryable<ProblemaVM> ToViewModel(this IQueryable<ProblemaResponse> problemas)
    {
        return problemas.Select(problema => problema.ToViewModel()).AsQueryable();
    }
    public static IQueryable<SetorVM> ToViewModel(this IQueryable<SetorResponse> setores)
    {
        return setores.Select(setor => setor.ToViewModel()).AsQueryable();
    }
    public static IQueryable<FuncionarioVM> ToViewModel(this IQueryable<FuncionarioResponse> funcionarios)
    {
        return funcionarios.Select(funcionario => funcionario.ToViewModel()).AsQueryable();
    }
    //--IColletionResponse para IColletionViewModel--//
    public static ICollection<CargoVM> ToViewModel(this ICollection<CargoResponse> cargos)
    {
        return cargos.Select(cargo => cargo.ToViewModel()).ToList();
    }
    public static ICollection<SetorVM> ToViewModel(this ICollection<SetorResponse> setores)
    {
        return setores.Select(setor => setor.ToViewModel()).ToList();
    }
    public static ICollection<ProblemaVM> ToViewModel(this ICollection<ProblemaResponse> problemas)
    {
        return problemas.Select(problema => problema.ToViewModel()).ToList();
    }
    public static ICollection<FuncionarioVM> ToViewModel(this ICollection<FuncionarioResponse> funcionarios)
    {
        return funcionarios.Select(funcionario => funcionario.ToViewModel()).ToList();
    }



}
