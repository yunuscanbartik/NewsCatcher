using Microsoft.Extensions.Options;
using NewsCatcher.Domain.Models.Config;
using NewsCatcher.Domain.Interfaces;
using System.IO;
using System.Reflection;
using System.Net.Mail;

namespace NewsCatcher.Application.Services
{
    public class SendEmailService : IEmailService
    {
        private readonly SmtpSettingsOptions _smtpSettings;
        private const string OtpTemplateResourceName = "NewsCatcher.Application.Templates.OtpVerificationTemplate.html";


        public SendEmailService(IOptions<SmtpSettingsOptions> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }
        private string OtpHtmlBody(string subject, string body)
        {
            var accessAssembly = Assembly.GetExecutingAssembly();
            using var OpenStream = accessAssembly.GetManifestResourceStream(OtpTemplateResourceName);
            if (OpenStream is null)
            {
                throw new FileNotFoundException($"Embedded OTP template was not found: {OtpTemplateResourceName}");
            }
            using var contentReader = new StreamReader(OpenStream);
            var rawHtmlTemplate = contentReader.ReadToEnd();
            return rawHtmlTemplate
                .Replace("{{SUBJECT}}", subject)
                .Replace("{{BODY}}", body);
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentException("Email address cannot be empty.", nameof(to));
            }
            var toAddress = to.Trim();
            var fromAddress = string.IsNullOrWhiteSpace(_smtpSettings.From)
                ? _smtpSettings.Username?.Trim()
                : _smtpSettings.From.Trim();

            if (string.IsNullOrWhiteSpace(fromAddress))
            {
                throw new ArgumentException("SMTP sender address cannot be empty. Check SmtpSettings:From.");
            }

            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress, "NewsCatcher"),
                Subject = subject,
                Body = OtpHtmlBody(subject, body),
                IsBodyHtml = true
            };
            mailMessage.To.Add(toAddress);

            await client.SendMailAsync(mailMessage);
            return true;
        }
    }
}





