using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Domain.Entities;

public record struct ComentarioId(Guid Value)
{
    public static ComentarioId Empty { get; } = default;
    public static ComentarioId CreateNew() => new(Guid.NewGuid());
    public readonly override string ToString() => StrongIdHelper<ComentarioId, Guid>.Serialize(Value);
    public static ComentarioId? TryParse(string? value) => StrongIdHelper<ComentarioId, Guid>.Deserialize(value);

}
public class Comentario : Entity<ComentarioId>
{
    public DateTime DataDoComentario { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public Anexo? AnexoComentario { get; private set; }
    public AnexoId? AnexoComentarioId { get; private set; }
    public AnomaliaId AnomaliaId { get; private set; } = AnomaliaId.Empty;
    public Anomalia? Anomalia { get; private set; } = null;
    public FuncionarioId ComentadorId { get; private set; } = FuncionarioId.Empty;
    public Funcionario? Comentador { get; private set; }

    protected Comentario(ComentarioId id) : base(id) { }

    private Comentario(ComentarioId id, string descricao, AnomaliaId anomaliaId, FuncionarioId comentadorId, DateTime dataDoComentario) : base(id)
    {
        Descricao = descricao;
        AnomaliaId = anomaliaId;
        ComentadorId = comentadorId;
        DataDoComentario = dataDoComentario;
    }

    internal static Result<Comentario> Create(string descricao, AnomaliaId anomaliaId, Guid comentaristaId, DateTime dataDoComentario)
    {
        if (string.IsNullOrEmpty(descricao) || anomaliaId == AnomaliaId.Empty || comentaristaId == Guid.Empty)
            return Result.Failure<Comentario>(DomainErrors.ComentarioErrors.Create);
        return new Comentario(ComentarioId.CreateNew(), descricao, anomaliaId, new(comentaristaId), dataDoComentario);
    }

    public Result AddAnexoComentario(string filename, string contentType, byte[] dados)
    {
        var anexo = Anexo.Create(filename, contentType,dados);
        if (anexo.IsFailure) return Result.Failure(anexo.Errors);
        AnexoComentario = anexo.Value;
        AnexoComentarioId = AnexoComentario.Id;
        return Result.Success();
    }

}
