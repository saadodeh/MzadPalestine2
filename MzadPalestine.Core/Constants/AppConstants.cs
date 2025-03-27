namespace MzadPalestine.Core.Constants;

public static class AppConstants
{
    public static class Auth
    {
        public const int AccessTokenExpirationMinutes = 15;
        public const int RefreshTokenExpirationDays = 7;
        public const int PasswordResetTokenExpirationHours = 24;
        public const int EmailConfirmationTokenExpirationHours = 24;
    }

    public static class Auction
    {
        public const int MinimumAuctionDurationHours = 1;
        public const int MaximumAuctionDurationDays = 30;
        public const int AutoExtendTimeMinutes = 5;
        public const decimal MinimumBidIncrement = 1.0m;
        public const int MaximumImagesPerListing = 10;
        public const int EndingSoonThresholdMinutes = 15;
    }

    public static class Cache
    {
        public const int DefaultExpirationMinutes = 60;
        public const string ActiveAuctionsKey = "active_auctions";
        public const string CategoryListKey = "categories";
        public const string LocationListKey = "locations";
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 50;
        public const int DefaultPage = 1;
    }

    public static class SignalR
    {
        public const string AuctionHub = "/auctionHub";
        public const string NotificationHub = "/notificationHub";
        public const string ChatHub = "/chatHub";
    }

    public static class FileUpload
    {
        public const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        public const string AllowedImageExtensions = ".jpg,.jpeg,.png";
        public const string UploadDirectory = "uploads";
    }
}
