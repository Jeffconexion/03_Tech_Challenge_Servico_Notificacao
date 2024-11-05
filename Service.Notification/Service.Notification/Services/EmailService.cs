using MailKit.Net.Smtp;
using MimeKit;
using Service.Notification.Services.Contracts;

namespace Service.Notification.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string name, string email, string body)
        {
            // instanciar classe de mensagem 'mimemessage' 
            var message = new MimeMessage();

            //from address
            message.From.Add(new MailboxAddress("Teste", "treinamentoaws202302@gmail.com"));

            // subject
            message.Subject = "Mensagem Web API";

            //to address
            message.To.Add(new MailboxAddress(name, email));

            //body
            message.Body = new TextPart("html")
            {
                Text = body,
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);

                client.Authenticate("treinamentoaws202302@gmail.com", "hG1!k26NLtp");

                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}
