using MailKit.Net.Smtp;
using MimeKit;
public class EmailService
{
    public async Task SendConfirmEmailAsync(string email, string subject, string message)
    {
        /*
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Администрация сайта", "adress"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("server", 465, true);
            await client.AuthenticateAsync("adress", "password");
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }*/
    }
}
