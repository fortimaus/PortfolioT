using PortfolioT.DataContracts.BindingModels;

namespace PortfolioT.MailWorker
{
    public abstract class AbstractMailWorker
    {
        protected string _mailLogin = string.Empty;

        protected string _mailPassword = string.Empty;

        protected string _smtpClientHost = string.Empty;

        protected int _smtpClientPort;

        protected string _popHost = string.Empty;

        protected int _popPort;

        public void MailConfig(MailConfigBindingModel config)
        {
            _mailLogin = config.MailLogin;
            _mailPassword = config.MailPassword;
            _smtpClientHost = config.SmtpClientHost;
            _smtpClientPort = config.SmtpClientPort;
            _popHost = config.PopHost;
            _popPort = config.PopPort;
        }

        public async void MailSendAsync(MailSendInfoBindingModel info)
        {
            if (string.IsNullOrEmpty(_mailLogin) || string.IsNullOrEmpty(_mailPassword))
            {
                return;
            }

            if (string.IsNullOrEmpty(_smtpClientHost) || _smtpClientPort == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(info.MailAddress) || string.IsNullOrEmpty(info.Subject) || string.IsNullOrEmpty(info.Text))
            {
                return;
            }

            await SendMailAsync(info);
        }

        
        protected abstract Task SendMailAsync(MailSendInfoBindingModel info);

    }
}
