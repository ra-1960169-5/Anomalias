namespace Anomalias.Shared;
public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "valor nulo fornecido!");
    public static implicit operator Result(Error error) => Result.Failure(error);
    public Result ToResult() => Result.Failure(this);
}
