namespace Anomalias.Application.Anexos.Query;
public sealed record class AnexoResponse(string ContentType, string FileName, byte[] Dados);
