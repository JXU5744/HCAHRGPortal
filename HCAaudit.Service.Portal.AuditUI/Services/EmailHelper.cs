using HCAaudit.Service.Portal.AuditUI.Models;
using HCAaudit.Service.Portal.AuditUI.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;

namespace HCAaudit.Service.Portal.AuditUI.Services
{
    public class EmailHelper
    {
        public readonly SmtpClient SmtpClient;
        private readonly IErrorLog _log;

        public EmailHelper(IConfiguration configuration, IErrorLog log)
        {
            _log = log;
            SmtpClient = new SmtpClient(configuration["SmtpHost"], Convert.ToInt32(configuration["SmtpPort"]));
        }
        public void SendEmailNotification(EmailTemplate emailContent)
        {
            try
            {
                //Email Notification
                var message = new MailMessage();
                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;

                if (!string.IsNullOrEmpty(emailContent.SendTo))
                {
                    message.To.Add(emailContent.SendTo);
                    message.To.Add("girish.chavan@hcahealthcare.com");
                }
                message.From = new MailAddress(emailContent.SendFrom, emailContent.SendFromName);
                message.Subject = emailContent.Subject;
                message.Body = emailContent.EmailBody;
                message.ReplyToList.Add(emailContent.ReplyTo);
                SmtpClient.Send(message);
            }
            catch (Exception ex)
            {
                _log.WriteErrorLog(new LogItem { ErrorType = "Error", ErrorSource = "EmailHelper_SendEmailNotification", ErrorDiscription = ex.Message });
            }
        }

    }
}
