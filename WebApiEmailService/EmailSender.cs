using MailKit.Net.Smtp;
using MimeKit;

namespace WebApiEmailService
{
    /// <summary>
    /// Class that contains email fields when using MailKit.
    /// </summary>   
    public class Message
    {
        /// <summary>
        /// Field Resipients (To) for email.
        /// </summary>
        public List<MailboxAddress> To { get; set; }
        /// <summary>
        /// Field Subject for email.
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Field Body (Content) for email.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Constructor for Message class.
        /// </summary>
        /// <param name="to">To email field(s).</param>
        /// <param name="subject">Subject email field.</param>
        /// <param name="content">Content email field.</param>
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }
    }
    /// <summary>
    /// Interface for email sending service.
    /// </summary>
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
    /// <summary>
    /// Class for email sending using MailKit.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        /// <summary>
        /// Constructor for EmailSender class.
        /// </summary>
        /// <param name="emailConfig">Email configuration.</param>
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        /// <summary>
        /// Async method for email sending using MailKit.
        /// </summary>
        /// <param name="message">Message class for MailKit.</param>
        /// <returns>Task from asyc method.</returns>
        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }

                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
