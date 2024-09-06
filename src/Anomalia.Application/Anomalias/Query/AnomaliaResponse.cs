namespace Anomalias.Application.Anomalias.Query;
public sealed record AnomaliaResponse
{
    public AnomaliaResponse(string id,
      int numeroRegistro,
      string? setor,
      string? responsavelSetor,
      string? problema,
      string questionamento,
      string resultadoEsperado,
      string consideracoesFinais,
      DateTime dataAbertura,
      DateTime? dataEncerramento,
      string usuarioAbertura,
      string? usuarioEncerramento,
      int status,
      bool restrita,
      string? anexoId,
      List<ComentarioResponse> comentarios
      )
    {
        Id = id;
        NumeroRegistro = numeroRegistro;
        Setor = setor;
        ResponsavelSetor = responsavelSetor;
        Problema = problema;
        Questionamento = questionamento;
        ResultadoEsperado = resultadoEsperado;
        ConsideracoesFinais = consideracoesFinais;
        DataAbertura = dataAbertura;
        DataEncerramento = dataEncerramento;
        UsuarioAbertura = usuarioAbertura;
        UsuarioEncerramento = usuarioEncerramento;
        Status = status;
        Restrita = restrita;
        AnexoId = anexoId;
        Comentarios = comentarios;

    }

    public string Id { get; init; }
    public int NumeroRegistro { get; init; }
    public string? Setor { get; init; } = string.Empty;
    public string? ResponsavelSetor { get; init; } = string.Empty;
    public string? Problema { get; init; } = string.Empty;
    public string Questionamento { get; init; } = string.Empty;
    public string ResultadoEsperado { get; init; } = string.Empty;
    public string ConsideracoesFinais { get; init; } = string.Empty;
    public DateTime DataAbertura { get; init; }
    public DateTime? DataEncerramento { get; init; }
    public string UsuarioAbertura { get; init; } = string.Empty;
    public string? UsuarioEncerramento { get; init; } = string.Empty;
    public int Status { get; init; }
    public bool Restrita { get; init; }
    public string? AnexoId { get; init; } = null;
    public List<ComentarioResponse> Comentarios { get; init; }

}


public sealed record ComentarioResponse
{

    public ComentarioResponse(string id, string descricao, string? anexoId, DateTime? data, string comentadoPor)
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

}

