using MzadPalestine.Core.Exceptions;

namespace MzadPalestine.Core.Helpers;

public static class Guard
{
    public static void AgainstNull<T>(T value, string name) where T : class
    {
        if (value == null)
        {
            throw new AppException($"{name} cannot be null");
        }
    }

    public static void AgainstNullOrEmpty(string value, string name)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new AppException($"{name} cannot be null or empty");
        }
    }

    public static void AgainstInvalidId(int id, string name)
    {
        if (id <= 0)
        {
            throw new AppException($"Invalid {name} ID: {id}");
        }
    }

    public static void AgainstNegativeOrZero(decimal value, string name)
    {
        if (value <= 0)
        {
            throw new AppException($"{name} must be greater than zero");
        }
    }

    public static void AgainstInvalidDateRange(DateTime startDate, DateTime endDate, string message)
    {
        if (startDate >= endDate)
        {
            throw new AppException(message);
        }
    }

    public static void AgainstFutureDate(DateTime date, string name)
    {
        if (date > DateTime.UtcNow)
        {
            throw new AppException($"{name} cannot be in the future");
        }
    }

    public static void AgainstPastDate(DateTime date, string name)
    {
        if (date < DateTime.UtcNow)
        {
            throw new AppException($"{name} cannot be in the past");
        }
    }
}
