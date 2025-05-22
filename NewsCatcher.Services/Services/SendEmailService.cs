using Microsoft.Extensions.Configuration;
using MimeKit;
using NewsCatcher.Services.Data;
using NewsCatcher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Services
{
    public class SendEmailService : IEmailService
    {   
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly string _from;


        public SendEmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["SmtpSettings:Host"];
            _smtpPort = int.Parse(configuration["SmtpSettings:Port"]);
            _enableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"]);
            _smtpUsername = configuration["SmtpSettings:Username"];
            _smtpPassword = configuration["SmtpSettings:Password"];
            _from = configuration["SmtpSettings:From"];
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    EnableSsl = _enableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(_smtpUsername, _smtpPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_from, "NewsCatcher"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-posta gönderilirken hata: {ex.Message}");
                return false;
            }

        }
    }
}
