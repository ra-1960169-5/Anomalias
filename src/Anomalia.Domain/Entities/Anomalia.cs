using Anomalias.Domain.Enums;
using Anomalias.Domain.Errors;
using Anomalias.Domain.Events;
using Anomalias.Shared;

namespace Anomalias.Domain.Entities;

public record struct AnomaliaId(Guid Value)
{
    public static AnomaliaId Empty { get; } = default;
    public static AnomaliaId CreateNew() => new(Guid.NewGuid());
    public readonly override string ToString() => StrongIdHelper<AnomaliaId, Guid>.Serialize(Value);
    public static AnomaliaId? TryParse(string? value) => StrongIdHelper<AnomaliaId, Guid>.Deserialize(value);
    public static bool TryCreate(string value, out AnomaliaId valueId) => (valueId = TryParse(value) ?? default) != default;
}

public class Anomalia : AggregateRoot<AnomaliaId>, IAggregateRoot
{
    public int Numero { get; private set; }
    public Problema? Problema { get; private set; }
    public ProblemaId ProblemaId { get; private set; } = ProblemaId.Empty;
    public Setor? Setor { get; private set; }
    public SetorId SetorId { get; private set; } = SetorId.Empty;
    public Anexo? AnexoAnomalia { get; private set; }
    public AnexoId? AnexoAnomaliaId { get; private set; }
    public DateTime DataDeAbertura { get; private set; }
    public DateTime? DataDeEncerramento { get; private set; }
    public Funcionario? FuncionarioAbertura { get; private set; }
    public Funcionario? FuncionarioEncerramento { get; private set; }
    public FuncionarioId AberturaId { get; private set; } = FuncionarioId.Empty;
    public FuncionarioId? EncerramentoId { get; private set; }
    public bool Restrita { get; private set; }
    public string Questionamento { get; private set; } = string.Empty;
    public string ResultadoEsperado { get; private set; } = string.Empty;
    public string ConsideracoesFinais { get; private set; } = string.Empty;

    private readonly ICollection<Comentario> _comentarios = [];
    public IReadOnlyCollection<Comentario> Comentarios => [.. _comentarios];
    public EStatus Status { get; set; } = EStatus.Andamento;

    protected Anomalia(AnomaliaId id) : base(id) { }

    private Anomalia(AnomaliaId id, ProblemaId problemaId, SetorId setorId, bool restrita, DateTime dataDeAbertura, FuncionarioId aberturaId, string questionamento, string resultadoEsperado) : base(id)
    {
        ProblemaId = problemaId;
        SetorId = setorId;
        Restrita = restrita;
        DataDeAbertura = dataDeAbertura;
        AberturaId = aberturaId;
        Questionamento = questionamento;
        ResultadoEsperado = resultadoEsperado;
    }
    public static Result<Anomalia> Create(string problemaId, string setorId, bool restrita, DateTime dataDeAbertura, Guid usuarioAberturaId, string questionamento, string resultadoEsperado)
    {
        if (!ProblemaId.TryCreate(problemaId, out ProblemaId problemaIdValue)
         || !SetorId.TryCreate(setorId, out SetorId setorIdValue)
         || usuarioAberturaId == Guid.Empty
         || string.IsNullOrEmpty(questionamento)
         || string.IsNullOrEmpty(resultadoEsperado)
         || dataDeAbertura > DateTime.Now) return Result.Failure<Anomalia>(DomainErrors.AnomaliaErrors.Create);
        var anomalia = new Anomalia(AnomaliaId.CreateNew(), problemaIdValue, setorIdValue, restrita, dataDeAbertura, new(usuarioAberturaId), questionamento, resultadoEsperado);
        anomalia.AddDomainEvent(new AnomaliaCreated(anomalia.Id));
        return anomalia;
    }

    public  Result<Comentario> AddComentario(string descricao, Guid comentaristaId, DateTime dataDoComentario)
    {
        var comentario = Comentario.Create(descricao, Id, comentaristaId, dataDoComentario);
        if (comentario.IsFailure) return Result.Failure<Comentario>(comentario.Errors);      
        _comentarios.Add(comentario.Value);
        AddDomainEvent(new ComentarioCreated(comentario.Value.Id));
        return Result.Success(comentario.Value);
    }

    public Result AddAnexoAnomalia(string fileName, string contentType, byte[] dados)
    {
        var anexo = Anexo.Create(fileName, contentType,dados);
        if (anexo.IsFailure) return Result.Failure(anexo.Errors);
        AnexoAnomalia = anexo.Value;
        AnexoAnomaliaId = anexo.Value.Id;
        AddDomainEvent(new AnexoCreated(anexo.Value.Id));
        return Result.Success();
    }


    public Result EndAnomalia(DateTime dataDeEncerramento, Guid userId, string consideracoesFinais)
    {
        if (userId == Guid.Empty || DataDeAbertura > dataDeEncerramento || Status == EStatus.Encerrado || string.IsNullOrEmpty(consideracoesFinais)) return DomainErrors.AnomaliaErrors.CannotEnd;

        EncerramentoId = new(userId);
        DataDeEncerramento = dataDeEncerramento;
        Status = EStatus.Encerrado;
        ConsideracoesFinais = consideracoesFinais;
        return Result.Success();
    }
}


