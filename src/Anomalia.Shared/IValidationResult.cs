namespace Anomalias.Shared;
public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "Ocorreu um problema de validação");

    Error[] Errors { get; }
}
