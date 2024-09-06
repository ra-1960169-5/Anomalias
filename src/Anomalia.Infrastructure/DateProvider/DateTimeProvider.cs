using Anomalias.Application.Abstractions.Services;

namespace Anomalias.Infrastructure.DateProvider;
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime DateOfTheFirstDayOfTheMonth => new(Now.Year, Now.Month, 1);
}
