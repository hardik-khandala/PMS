using System.Net.Mail;
using System.Net;

namespace Server.Services
{
    public class SMTPService
    {
        private readonly IConfiguration _configuration;
        public SMTPService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["SMTP:Host"])
                {
                    Port = 587, // Use 465 if SSL is required
                    Credentials = new NetworkCredential(_configuration["SMTP:UserName"], _configuration["SMTP:Password"]),
                    EnableSsl = true // Use false if the server doesn't support SSL
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["SMTP:MailAddress"]),
                    Subject = subject,
                    Body = body,    
                    IsBodyHtml = true, // Set to false if sending plain text emails

                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
