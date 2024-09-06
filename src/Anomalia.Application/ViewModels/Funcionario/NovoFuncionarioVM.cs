using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;

public class NovoFuncionarioVM
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [EmailAddress(ErrorMessage = "É necessário um endereço de e-mail válido!")]
    [RegularExpression(@"^[\w-_]+(\.[\w!#$%'*+\/=?\^`{|}]+)*@((([\-\w]+\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "É necessário um endereço de e-mail válido!")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Setor")]

    public string SetorId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Cargo")]

    public string CargoId { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [StringLength(15, ErrorMessage = "O campo {0} é precisar ter entre {2} e {1} caracteres!", MinimumLength = 6)]
    [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)).+$", ErrorMessage = "A Senha deve conter letras maiúscula, minúscula, numero e caracter especial")]
    [DataType(DataType.Password)]
    [DisplayName("Senha")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirme a Senha")]
    [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem!")]
    [StringLength(15, ErrorMessage = "O campo {0} é precisar ter entre {2} e {1} caracteres!", MinimumLength = 6)]
    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Gestor?")]
    public bool Gestor { get; set; }

    public ICollection<CargoVM> Cargos { get; set; } = [];

    public ICollection<SetorVM> Setores { get; set; } = [];
}

