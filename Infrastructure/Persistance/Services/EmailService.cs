using System.Net.Mail;
using Application.Services;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Persistance.Services;

public class EmailService  : IEmailService
{
    public async Task SendFaultEmailAsync(string emailAdress, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("ahmetguvendik011348@gmail.com"));
        email.To.Add(MailboxAddress.Parse(emailAdress));
        email.Subject = "Arizaniz Hakkinda";
        email.Body = new TextPart(TextFormat.Plain) { Text = body };
        using var smtp = new SmtpClient();  
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("ahmetguvendik011348@gmail.com", "avfe nhlb iwfj qokg");
        smtp.Send(email);
        smtp.Disconnect(true);
    }

    public async Task SendSupervisorToTeknisyenEmailAsync(string emailAdress, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("ahmetguvendik011348@gmail.com"));
        email.To.Add(MailboxAddress.Parse(emailAdress));
        email.Subject = "Arizaniz Hakkinda";
        email.Body = new TextPart(TextFormat.Plain) { Text = body };
        using var smtp = new SmtpClient();  
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("ahmetguvendik011348@gmail.com", "avfe nhlb iwfj qokg");
        smtp.Send(email);
        smtp.Disconnect(true);
    }

    public async Task SendClosedFaultEmailAsync(string emailAdress, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("ahmetguvendik011348@gmail.com"));
        email.To.Add(MailboxAddress.Parse(emailAdress));
        email.Subject = "Arizaniz Hakkinda";    
        email.Body = new TextPart(TextFormat.Plain) { Text = body };
        using var smtp = new SmtpClient();  
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("ahmetguvendik011348@gmail.com", "avfe nhlb iwfj qokg");  
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}