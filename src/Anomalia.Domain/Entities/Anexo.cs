using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Domain.Entities;

public readonly record struct AnexoId(Guid Value)
{
    public static AnexoId Empty { get; } = default;
    public static AnexoId NewAnexoId() => new(Guid.NewGuid());
    public readonly override string ToString() => StrongIdHelper<AnexoId, Guid>.Serialize(Value);
    public static AnexoId? TryParse(string? value) => StrongIdHelper<AnexoId, Guid>.Deserialize(value);
}
public class Anexo : Entity<AnexoId>
{
    public string Nome { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;   
    public byte[] Dados { get; private set; } = [];


    protected Anexo(AnexoId id) : base(id) { }


    private Anexo(AnexoId id, string nome, string contentype, byte[] dados) : base(id)
    {

        Nome = nome;
        ContentType = contentype;
        Dados = dados;
    }


    internal static Result<Anexo> Create(string nome, string contentype, byte[] dados)
    {
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(contentype) || (dados is null || dados.Length==0))
            return Result.Failure<Anexo>(DomainErrors.AnexoErrors.Create);
        return new Anexo(AnexoId.NewAnexoId(), nome, contentype, dados);
    }

}
