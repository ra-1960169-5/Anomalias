using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;
public class ForgotPasswordVM
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
