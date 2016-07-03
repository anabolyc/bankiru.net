using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using NLog;

namespace www.BankBals.MailSender {

    public struct Config {
        public bool EnableMail;
        public bool EnableSsl;
        public string SmtpServer;
        public short SmtpPort;
        public string SmtpUserName;
        public string SmtpPassword;
        public string SmtpReply;
        public string SmtpReplyFullName;
    }

    public static class MailSender {

        private static NLog.Logger logger = NLog.LogManager.GetLogger("Gmail");

        public static void SendMail(Config config, string email, string subject, string body, MailAddress mailAddress = null) {

            //try {
                if (config.EnableMail) {
                    if (mailAddress == null) {
                        mailAddress = new MailAddress(config.SmtpReply, config.SmtpReplyFullName);
                    }
                    MailMessage message = new MailMessage(
                        mailAddress,
                        new MailAddress(email)) {
                            Subject = subject,
                            BodyEncoding = Encoding.UTF8,
                            Body = body,
                            IsBodyHtml = true,
                            SubjectEncoding = Encoding.UTF8
                        };
                    SmtpClient client = new SmtpClient {
                        Host = config.SmtpServer,
                        Port = config.SmtpPort,
                        UseDefaultCredentials = false,
                        EnableSsl = config.EnableSsl,
                        Credentials =
                            new NetworkCredential(config.SmtpUserName,
                                                  config.SmtpPassword),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    client.Send(message);
                } else {
                    logger.Debug("Email : {0} {1} \t Subject: {2} {3} Body: {4}", email, Environment.NewLine, subject, Environment.NewLine, body);
                }
            //} catch (Exception ex) {
            //    logger.Error("Mail send exception", ex.Message);
            //}
        }
    }
}