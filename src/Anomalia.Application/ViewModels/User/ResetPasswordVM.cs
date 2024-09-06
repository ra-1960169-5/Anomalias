using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;
public class ResetPasswordVM
{
    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [StringLength(10, ErrorMessage = "O campo {0} é precisar ter entre {2} e {1} caracteres!", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Nova Senha")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirme a Senha")]
    [Compare("Password", ErrorMessage = "A nova senha e a senha de confirmação não correspondem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;
}
