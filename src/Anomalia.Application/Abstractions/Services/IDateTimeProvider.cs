namespace Anomalias.Application.Abstractions.Services;
public interface IDateTimeProvider
{
    public DateTime Now { get; }

    public DateTime DateOfTheFirstDayOfTheMonth { get; }
}
