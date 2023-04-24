using System.Net.Mail;
using System.Net.Mime;
using System.Net;

public static class EmailHandler
{
    static string email = "SpirallingUp.Data@hotmail.com";
    static string password = "rJ6R5aJwRbXCNQ9YrLNC";
    static string smtpClient = "smtp-mail.outlook.com";

    public static async void SendEmail(string file, string subject = "")
    {
        MailMessage mail = new();
        mail.From = new MailAddress(email);
        mail.To.Add(email);
        mail.Body = string.Empty;
        mail.Subject = subject;
        mail.Attachments.Add(new Attachment(file, MediaTypeNames.Text.Plain));

        SmtpClient smtp = new(smtpClient, 587)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(email, password) as ICredentialsByHost,
            EnableSsl = true
        };

        smtp.Send(mail);
        mail.Body += " async";
        await smtp.SendMailAsync(mail);
        smtp.Dispose();
    }

    public static async void Send(string message, string subject = "")
    {
        MailMessage mail = new();
        mail.From = new MailAddress(email);
        mail.To.Add(email);
        mail.Body = message;
        mail.Subject = subject;

        SmtpClient smtp = new(smtpClient, 587)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(email, password) as ICredentialsByHost,
            EnableSsl = true
        };

        smtp.Send(mail);
        mail.Body += " async";
        await smtp.SendMailAsync(mail);
        smtp.Dispose();
    }
}
