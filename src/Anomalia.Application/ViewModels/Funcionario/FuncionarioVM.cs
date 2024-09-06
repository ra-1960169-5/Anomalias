using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;
public class FuncionarioVM
{
    public FuncionarioVM(string id, string nome, string setorId, string cargoId, bool ativo, bool gestor)
    {
        Id = id;
        Nome = nome;
        SetorId = setorId;
        CargoId = cargoId;
        Ativo = ativo;
        Gestor = gestor;
    }

    public FuncionarioVM() { }

    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Setor")]
    public string SetorId { get; set; } = string.Empty;

    [Display(Name = "Cargo")]
    public string CargoId { get; set; } = string.Empty;

    [Display(Name = "Ativo?")]
    public bool Ativo { get; set; }

    [Display(Name = "Gestor?")]
    public bool Gestor { get; set; }

}
