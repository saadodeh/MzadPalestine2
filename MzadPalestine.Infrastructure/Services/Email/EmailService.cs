using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Settings;

namespace MzadPalestine.Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var builder = new BodyBuilder();
        if (isHtml)
            builder.HtmlBody = body;
        else
            builder.TextBody = body;

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendWelcomeEmailAsync(string to, string userName, string confirmationLink)
    {
        var subject = "مرحباً بك في مزاد فلسطين";
        var body = $@"
            <h2>مرحباً {userName}،</h2>
            <p>شكراً لتسجيلك في مزاد فلسطين. نرجو تأكيد بريدك الإلكتروني للبدء بالمزايدة والبيع.</p>
            <p>اضغط على الرابط التالي لتأكيد حسابك:</p>
            <p><a href='{confirmationLink}'>تأكيد الحساب</a></p>
            <p>مع تحيات،<br>فريق مزاد فلسطين</p>";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        var subject = "إعادة تعيين كلمة المرور - مزاد فلسطين";
        var body = $@"
            <h2>إعادة تعيين كلمة المرور</h2>
            <p>لقد تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بحسابك.</p>
            <p>اضغط على الرابط التالي لإعادة تعيين كلمة المرور:</p>
            <p><a href='{resetLink}'>إعادة تعيين كلمة المرور</a></p>
            <p>إذا لم تقم بطلب إعادة تعيين كلمة المرور، يمكنك تجاهل هذا البريد.</p>
            <p>مع تحيات،<br>فريق مزاد فلسطين</p>";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendAuctionWonEmailAsync(string to, string userName, int auctionId, decimal winningBid)
    {
        var subject = "تهانينا! لقد ربحت المزاد";
        var body = $@"
            <h2>تهانينا {userName}!</h2>
            <p>لقد ربحت المزاد رقم {auctionId} بقيمة {winningBid:C}.</p>
            <p>يرجى إكمال عملية الدفع في أقرب وقت ممكن.</p>
            <p>مع تحيات،<br>فريق مزاد فلسطين</p>";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendBidOutbidEmailAsync(string to, string userName, int auctionId, decimal newBid)
    {
        var subject = "تم تجاوز مزايدتك";
        var body = $@"
            <h2>مرحباً {userName}،</h2>
            <p>تم تجاوز مزايدتك في المزاد رقم {auctionId}.</p>
            <p>المزايدة الجديدة هي: {newBid:C}</p>
            <p>يمكنك العودة للمزاد والمزايدة مرة أخرى.</p>
            <p>مع تحيات،<br>فريق مزاد فلسطين</p>";

        await SendEmailAsync(to, subject, body);
    }

    public async Task SendAuctionEndingSoonEmailAsync(string to, string userName, int auctionId, TimeSpan timeRemaining)
    {
        var subject = "المزاد سينتهي قريباً";
        var body = $@"
            <h2>مرحباً {userName}،</h2>
            <p>المزاد رقم {auctionId} سينتهي خلال {timeRemaining.TotalMinutes} دقيقة.</p>
            <p>إذا كنت مهتماً، يرجى المزايدة قبل انتهاء الوقت.</p>
            <p>مع تحيات،<br>فريق مزاد فلسطين</p>";

        await SendEmailAsync(to, subject, body);
    }
}
