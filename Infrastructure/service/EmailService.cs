using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["EmailSettings:SMTPHost"])
                {
                    Port = int.Parse(_configuration["EmailSettings:SMTPPort"]),
                    Credentials = new NetworkCredential(_configuration["EmailSettings:SMTPUser"], _configuration["EmailSettings:SMTPPass"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:SMTPUser"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                // Send the email asynchronously
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the error (you could use a logger here)
                // For now, let's just throw an exception
                throw new InvalidOperationException("An error occurred while sending the email.", ex);
            }
        }
    }
}
