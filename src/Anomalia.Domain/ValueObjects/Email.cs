using Anomalias.Domain.Errors;
using Anomalias.Shared;
using System.Text.RegularExpressions;

namespace Anomalias.Domain.ValueObjects;
public class Email
{
    public const int MaxLength = 254;
    public const int MinLength = 5;
    public string Value { get; private set; } = string.Empty;
    protected Email() { }
    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {

        return Result.Ensure(
              email,
              (e => !string.IsNullOrWhiteSpace(e), DomainErrors.EmailErrors.Empty),
              (e => e.Length <= MaxLength, DomainErrors.EmailErrors.TooLong),
              (e => IsValid(e), DomainErrors.EmailErrors.InvalidFormat))
               .Map(e => new Email(e));

 
    }

    public static bool IsValid(string value)
    {
        var expression = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
        return Regex.IsMatch(value, expression);
    }



}
