using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Domain.Entities;

public record struct ProblemaId(Guid Value)
{
    public static ProblemaId Empty { get; } = default;
    public static ProblemaId CreateNew() => new(Guid.NewGuid());
    public readonly override string ToString() => StrongIdHelper<ProblemaId, Guid>.Serialize(Value);
    public static ProblemaId? TryParse(string? value) => StrongIdHelper<ProblemaId, Guid>.Deserialize(value);
    public static bool TryCreate(string value, out ProblemaId valueId) => (valueId = TryParse(value) ?? default) != default;
}
public class Problema : Entity<ProblemaId>
{
    public string Descricao { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;
    public ICollection<Anomalia> Anomalias { get; private set; } = [];
    protected Problema(ProblemaId id) : base(id) { }

    private Problema(ProblemaId id, string descricao) : base(id)
    {
        Descricao = descricao;
    }
    public static Result<Problema> Create(string descricao)
    {
        if (string.IsNullOrEmpty(descricao)) return Result.Failure<Problema>(DomainErrors.ProblemaErrors.Create);

        return new Problema(ProblemaId.CreateNew(), descricao);
    }
    public void Update(string decricao)
    {
        Descricao = decricao;
    }
}
