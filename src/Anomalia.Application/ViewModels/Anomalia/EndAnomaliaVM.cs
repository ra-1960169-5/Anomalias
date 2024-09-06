using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;
public sealed record EndAnomaliaVM
{
    public string AnomaliaId { get; init; } = string.Empty;

    public string NumeroRegistro { get; init; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [StringLength(500, ErrorMessage = "O campo {0} precisar ter entre {2} e {1} caracteres!", MinimumLength = 10)]
    [Display(Name = "Considerações Finais")]
    public string ConsideracoesFinais { get; init; } = string.Empty;
}
