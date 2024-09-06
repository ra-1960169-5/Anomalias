using Anomalias.Domain.Errors;
using Anomalias.Shared;

namespace Anomalias.Domain.Entities;

public record struct SetorId(Guid Value)
{

    public static SetorId Empty { get; } = default;
    public static SetorId CreateNew() => new(Guid.NewGuid());
    public readonly override string ToString() => StrongIdHelper<SetorId, Guid>.Serialize(Value);
    public static SetorId? TryParse(string? value) => StrongIdHelper<SetorId, Guid>.Deserialize(value);
    public static bool TryCreate(string value, out SetorId valueId) => (valueId = TryParse(value) ?? default) != default;
}
public class Setor : Entity<SetorId>
{
    public string Descricao { get; private set; } = string.Empty;
    public Funcionario? Gestor { get; private  set; }

    private readonly ICollection<Funcionario> _funcionarios = [];
    public IReadOnlyCollection<Funcionario> Funcionarios => [.. _funcionarios];
    public bool Ativo { get; private set; } = true;  
    protected Setor(SetorId id) : base(id) { }
    private Setor(SetorId id, string descricao, Funcionario? gestor=null) : base(id)
    {
        Descricao = descricao;
        Gestor = gestor;
    }

    public static Result<Setor> Create(string descricao)
    {
        if (string.IsNullOrEmpty(descricao)) return Result.Failure<Setor>(DomainErrors.SetorErrors.Create);
        return new Setor(SetorId.CreateNew(), descricao);
    }

    public static Result<Setor> Create(SetorId id,string descricao,Funcionario? gestor=null)
    {
        if (string.IsNullOrEmpty(descricao) || id == SetorId.Empty) return Result.Failure<Setor>(DomainErrors.SetorErrors.Create);
        return new Setor(id, descricao,gestor);
    }

    public Result Update(string descricao, Funcionario? gestor)
    {
        if (string.IsNullOrEmpty(descricao)) return Result.Failure<Setor>(DomainErrors.SetorErrors.Update);      
        Descricao = descricao;
        Gestor = gestor;      
        return Result.Success();
    }

    public Result CheckSetorManager(FuncionarioId? funcionarioId) {
        if (Gestor is null) return Result.Success();
        if (Gestor.Id == funcionarioId) return Result.Success();
        return Result.Failure(new Error("Setor.UpdateGestor", $"o setor {Descricao} ja possui um gestor"));
    }


 
}


