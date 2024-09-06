using Anomalias.Application.Extensions;
using System.ComponentModel.DataAnnotations;


namespace Anomalias.Application.ViewModels;
public sealed record ConsultAnomaliaVM
{

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Data Inicial")]
    public DateTime DataInicial { get; init; } = DateTime.Now.GetFirstDayWithMonth();

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Data Final")]
    public DateTime DataFinal { get; init; } = DateTime.Now.GetLastDayWithMonth();

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Status")]
    public int Status { get; init; }

    public IReadOnlyList<AnomaliaVM>? Anomalias { get; init; } = null;

    public IReadOnlyList<AnomaliaVM>? AnomaliasSetor { get; init; } = null;

    public IReadOnlyList<AnomaliaVM>? AnomaliasUser { get; init; } = null;


}
