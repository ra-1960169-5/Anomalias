using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Domain.Entities;

public record struct CargoId(Guid Value)
{
    public static CargoId Empty { get; } = default;
    public static CargoId CreateNew() => new(Guid.NewGuid());
    public readonly override string ToString() => StrongIdHelper<CargoId, Guid>.Serialize(Value);
    public static CargoId? TryParse(string? value) => StrongIdHelper<CargoId, Guid>.Deserialize(value);
    public static bool TryCreate(string value, out CargoId valueId) => (valueId = TryParse(value) ?? default) != default;
}
public class Cargo : Entity<CargoId>
{
    public string Descricao { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;
    protected Cargo(CargoId id) : base(id) { }
    private Cargo(CargoId id, string descricao) : base(id)
    {
        Descricao = descricao;
    }
    public static Result<Cargo> Create(string descricao)
    {
        if (string.IsNullOrEmpty(descricao)) return Result.Failure<Cargo>(DomainErrors.CargoErrors.Create);
        return new Cargo(CargoId.CreateNew(), descricao);
    }

    public static Result<Cargo> Restore(CargoId cargoId,string descricao)
    {
        if (string.IsNullOrEmpty(descricao) || cargoId == CargoId.Empty) return Result.Failure<Cargo>(DomainErrors.CargoErrors.Create);
        return new Cargo(cargoId, descricao);
    }

    public void Update(string decricao)
    {
        Descricao = decricao;
    }
}

