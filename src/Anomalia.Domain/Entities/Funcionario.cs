using Anomalias.Domain.Errors;
using Anomalias.Shared;
using System.ComponentModel;

namespace Anomalias.Domain.Entities;

public record struct FuncionarioId(Guid Value)
{
    public static FuncionarioId Empty { get; } = default;
    public static FuncionarioId CreateNew() => new(Guid.NewGuid());
    public static FuncionarioId Create(Guid value) => new(value);
    public readonly override string ToString() => StrongIdHelper<FuncionarioId, Guid>.Serialize(Value);
    public static FuncionarioId? TryParse(string? value) => StrongIdHelper<FuncionarioId, Guid>.Deserialize(value);
    public static bool TryParse(string value, out FuncionarioId valueId) => (valueId = TryParse(value) ?? default) != default;
    public static bool TryCreate(string? value, out FuncionarioId valueId)
    {
        if (!string.IsNullOrEmpty(value) && TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromString(value) is Guid guid)
        {
            valueId = new(guid);
            return true;
        }
        valueId = default;
        return false;
    }
}
public class Funcionario : AggregateRoot<FuncionarioId>, IAggregateRoot
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public SetorId SetorId { get; private set; } = SetorId.Empty;
    public Setor? Setor { get; private set; }
    public CargoId CargoId { get; private set; } = CargoId.Empty;
    public Cargo? Cargo { get; private set; }
    public bool Ativo { get; private set; } = true;  
    public SetorId? GestorSetorId { get; private set; }
    public ICollection<Comentario>? Comentarios { get; private set; } = [];
    public ICollection<Anomalia>? FuncionariosAbertura { get; private set; } = [];
    public ICollection<Anomalia>? FuncionariosEncerramento { get; private set; } = [];
    protected Funcionario(FuncionarioId id) : base(id) { }

    private Funcionario(FuncionarioId id, string nome, string email, SetorId setorId, CargoId cargoId, bool gestor) : base(id)
    {
        Nome = nome;
        Email = email;
        CargoId = cargoId;
        SetorId = setorId;       
        GestorSetorId = gestor ? SetorId : null;       
    }

    public static Result<Funcionario> Create(string id, string nome, string email, string setorId, string cargoId, bool gestor)
    {

        if (!FuncionarioId.TryParse(id, out FuncionarioId funcionarioIdValue) ||
            string.IsNullOrEmpty(nome) ||
            string.IsNullOrEmpty(email) ||
            !SetorId.TryCreate(setorId, out SetorId setorIdValue)
            || !CargoId.TryCreate(cargoId, out CargoId cargoIdValue)) return Result.Failure<Funcionario>(DomainErrors.FuncionarioErrors.Create);

        return Result.Success(new Funcionario(funcionarioIdValue, nome, email, setorIdValue, cargoIdValue, gestor));
    }

    public static Result<Funcionario> Create(string nome, string email, string setorId, string cargoId, bool gestor)
    {
        if (string.IsNullOrEmpty(nome) ||
            string.IsNullOrEmpty(email) ||
            !SetorId.TryCreate(setorId, out SetorId setorIdValue)
            || !CargoId.TryCreate(cargoId, out CargoId cargoIdValue)) return Result.Failure<Funcionario>(DomainErrors.FuncionarioErrors.Create);
        return Result.Success(new Funcionario(FuncionarioId.CreateNew(), nome, email, setorIdValue, cargoIdValue, gestor));
    }

    public Result Update(string nome, string setorId, string cargoId, bool ativo, bool gestor)
    {
        if (string.IsNullOrEmpty(nome)
       || !SetorId.TryCreate(setorId, out SetorId setorIdValue)
       || !CargoId.TryCreate(cargoId, out CargoId cargoIdValue)) return DomainErrors.FuncionarioErrors.Update;

        Nome = nome;
        CargoId = cargoIdValue;
        SetorId = setorIdValue;
        Ativo = ativo;       
        GestorSetorId = gestor ? SetorId : null;   
        return Result.Success();
    }

    public void DisableFuncionario() => Ativo = false;
    public void EnableFuncionario() => Ativo = true;
    public bool PossuiGestor() => GestorSetorId is not null;
    
}

