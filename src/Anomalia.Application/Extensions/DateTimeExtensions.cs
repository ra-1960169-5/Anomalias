namespace Anomalias.Application.Extensions;
public static class DateTimeExtensions
{
    public static DateTime GetFirstDayWithMonth(this DateTime date)
        => new(date.Year, date.Month, 1);
    public static DateTime GetLastDayWithMonth(this DateTime date)
        => date.GetFirstDayWithMonth().AddMonths(1).AddDays(-1);
}
