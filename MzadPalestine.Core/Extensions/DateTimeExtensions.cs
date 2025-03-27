namespace MzadPalestine.Core.Extensions;

public static class DateTimeExtensions
{
    public static bool IsExpired(this DateTime dateTime)
    {
        return dateTime < DateTime.UtcNow;
    }

    public static bool IsInFuture(this DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }

    public static bool IsWithinRange(this DateTime dateTime, DateTime startDate, DateTime endDate)
    {
        return dateTime >= startDate && dateTime <= endDate;
    }

    public static bool IsEndingSoon(this DateTime endTime, int thresholdMinutes = 15)
    {
        var timeRemaining = endTime - DateTime.UtcNow;
        return timeRemaining.TotalMinutes <= thresholdMinutes && timeRemaining.TotalMinutes > 0;
    }

    public static string ToArabicDateString(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("ar-SA"));
    }

    public static string ToArabicTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("hh:mm tt", new System.Globalization.CultureInfo("ar-SA"));
    }

    public static string ToArabicDateTimeString(this DateTime dateTime)
    {
        return $"{dateTime.ToArabicDateString()} {dateTime.ToArabicTimeString()}";
    }
}
