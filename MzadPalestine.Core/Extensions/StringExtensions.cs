using System.Text.RegularExpressions;

namespace MzadPalestine.Core.Extensions;

public static class StringExtensions
{
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Use a simple regex pattern for email validation
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Palestinian phone number format
        var pattern = @"^((?:\+?970|00970|0)(?:2|4|8|9)[0-9]{7})$";
        return Regex.IsMatch(phoneNumber, pattern);
    }

    public static string ToSafeFileName(this string fileName)
    {
        // Remove invalid file name characters
        var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
        var invalidRegex = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
        
        return Regex.Replace(fileName, invalidRegex, "_");
    }

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }

    public static string RemoveHtmlTags(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    public static bool IsValidImageExtension(this string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return false;

        var extension = Path.GetExtension(fileName).ToLower();
        return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
    }
}
