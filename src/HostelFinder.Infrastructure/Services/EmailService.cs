﻿using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HostelFinder.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string emailTo, string subject, string body)
        {
            MailMessage message = new MailMessage(_mailSettings.SenderEmail, emailTo, subject, body);
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;

            message.ReplyToList.Add(new MailAddress(_mailSettings.SenderEmail));
            using var smtpClient = new SmtpClient(_mailSettings.Server);
            smtpClient.Port = _mailSettings.Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_mailSettings.SenderEmail, _mailSettings.Password);
            try
            {
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message) ;
            }
        }
    }
}