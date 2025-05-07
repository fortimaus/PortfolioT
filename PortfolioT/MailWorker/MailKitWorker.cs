using PortfolioT.DataContracts.BindingModels;
using System.Net.Mail;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace PortfolioT.MailWorker
{
    public class MailKitWorker : AbstractMailWorker
    {
        private static MailKitWorker? _instance;

        public static MailKitWorker getInstance(MailConfigBindingModel config = null)
        {
            if(_instance == null)
            {
                _instance = new MailKitWorker();
                if (config == null)
                    throw new Exception("Проблемы с почтой");
                _instance.MailConfig(config);
            }
            return _instance;
        }

        protected override async Task SendMailAsync(MailSendInfoBindingModel info)
        {
            using var objMailMessage = new MailMessage();
            using var objSmtpClient = new SmtpClient(_smtpClientHost, _smtpClientPort);
            try
            {
                objMailMessage.From = new MailAddress(_mailLogin);
                objMailMessage.To.Add(new MailAddress(info.MailAddress));
                objMailMessage.Subject = info.Subject;
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Body = info.Text;
                objMailMessage.SubjectEncoding = Encoding.UTF8;
                objMailMessage.BodyEncoding = Encoding.UTF8;

                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(_mailLogin, _mailPassword);

                await Task.Run(() => objSmtpClient.Send(objMailMessage));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
