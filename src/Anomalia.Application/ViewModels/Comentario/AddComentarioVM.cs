using Anomalias.Application.Extensions;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Anomalias.Application.ViewModels;
public sealed record AddComentarioVM
{
    public string AnomaliaId { get; init; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é Obrigatório!")]
    [StringLength(500, ErrorMessage = "O campo {0} precisar ter entre {2} e {1} caracteres!", MinimumLength = 10)]
    [Display(Name = "Comentário")]
    public string Descricao { get; init; } = string.Empty;
    public IFormFile? Anexo { get; init; }
    public AnexoVM? AnexoVM => Anexo is not null ? new AnexoVM(Anexo.ContentType, Anexo.FileName, Anexo.ConvertFileToByte()) : null;
}


