namespace MzadPalestine.Core.Constants;

public static class ErrorMessages
{
    // Auth errors
    public const string InvalidCredentials = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
    public const string EmailAlreadyExists = "البريد الإلكتروني مستخدم مسبقاً";
    public const string PhoneAlreadyExists = "رقم الهاتف مستخدم مسبقاً";
    public const string UserNotFound = "المستخدم غير موجود";
    public const string EmailNotConfirmed = "يرجى تأكيد البريد الإلكتروني أولاً";
    public const string InvalidToken = "رمز التحقق غير صالح أو منتهي الصلاحية";

    // Listing errors
    public const string ListingNotFound = "المنتج غير موجود";
    public const string UnauthorizedListingAccess = "غير مصرح لك بالوصول لهذا المنتج";
    public const string CategoryNotFound = "الفئة غير موجودة";
    public const string LocationNotFound = "الموقع غير موجود";
    public const string InvalidListingStatus = "حالة المنتج غير صالحة";

    // Auction errors
    public const string AuctionNotFound = "المزاد غير موجود";
    public const string AuctionEnded = "المزاد منتهي";
    public const string AuctionNotStarted = "المزاد لم يبدأ بعد";
    public const string BidTooLow = "قيمة المزايدة يجب أن تكون أعلى من المزايدة الحالية";
    public const string CannotBidOnOwnAuction = "لا يمكنك المزايدة على منتجك";
    public const string BidNotFound = "المزايدة غير موجودة";
    public const string CannotCancelBid = "لا يمكن إلغاء المزايدة في هذه المرحلة";

    // Payment errors
    public const string PaymentNotFound = "عملية الدفع غير موجودة";
    public const string PaymentFailed = "فشلت عملية الدفع";
    public const string PaymentAlreadyProcessed = "تم معالجة عملية الدفع مسبقاً";
    public const string InsufficientFunds = "رصيد غير كافي";

    // General errors
    public const string InvalidOperation = "عملية غير صالحة";
    public const string ServerError = "حدث خطأ في النظام، يرجى المحاولة لاحقاً";
    public const string ValidationError = "البيانات المدخلة غير صالحة";
    public const string Unauthorized = "غير مصرح لك بتنفيذ هذه العملية";
}
