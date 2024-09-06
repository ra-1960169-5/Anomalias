using Anomalias.Application.Extensions;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;
public record RegisterAnomaliaVM
{
    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Setor")]
    public string SetorId { get; init; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Problema")]
    public string ProblemaId { get; init; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [StringLength(500, ErrorMessage = "O campo {0} é precisar ter entre {2} e {1} caracteres!", MinimumLength = 10)]
    [Display(Name = "Questionamento")]
    public string Questionamento { get; init; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "ResultadoEsperado")]
    [StringLength(500, ErrorMessage = "O campo {0} é precisar ter entre {2} e {1} caracteres!", MinimumLength = 10)]
    public string ResultadoEsperado { get; init; } = string.Empty;
    public IFormFile? Anexo { get; init; }
    public AnexoVM? AnexoVM => Anexo is not null ? new AnexoVM(Anexo.ContentType, Anexo.FileName, Anexo.ConvertFileToByte()) : null;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [Display(Name = "Anomalia Restrita?")]
    public bool Restrita { get; init; }

    public ICollection<ProblemaVM> Problemas { get; set; } = [];

    public ICollection<SetorVM> Setores { get; set; } = [];


}
