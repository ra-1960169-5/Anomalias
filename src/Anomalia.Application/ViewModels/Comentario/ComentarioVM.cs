namespace Anomalias.Application.ViewModels;
public sealed record ComentarioVM
{
    public ComentarioVM(string id, string descricao, string? anexoId, DateTime? data, string comentadoPor)
    {
        Id = id;
        Descricao = descricao;
        AnexoId = anexoId;
        Data = data;
        ComentadoPor = comentadoPor;
    }

    public string Id { get; init; }

    public string Descricao { get; init; }

    public string? AnexoId { get; init; }

    public DateTime? Data { get; init; }

    public string ComentadoPor { get; init; } = string.Empty;

    public bool PossuiAnexo() => string.IsNullOrEmpty(AnexoId) is false;
}
