using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using BrowserGameBackend.Types.Options;

namespace BrowserGameBackend.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends emails
        /// </summary>
        /// <param name="receiver">Recepient email</param>
        /// <param name="subject"></param>
        /// <param name="body">Body in plain text</param>
        /// <returns> Return options are "server error", "bad email" and "Ok"</returns>
        public string Send (string receiver, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailOptions emailOptions;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            emailOptions = new EmailOptions();
            _configuration.GetSection(EmailOptions.OptionName).Bind(emailOptions);

            if (emailOptions.Sender == null || emailOptions.SenderPass == null)
            {
                throw new ArgumentNullException("EmailOptions", "The configuration lacks sender data for EmailService");
            }

        }

        public string Send(string receiver, string subject, string body)
        {

            // create
            MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse(emailOptions.Sender));
            email.To.Add(MailboxAddress.Parse(receiver));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = body };
            // client
            using SmtpClient smtpClient = new();            
            try
            {
                smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine("Error trying to connect: {0}", ex.Message);
                Console.WriteLine("\tStatusCode: {0}", ex.StatusCode);
                return "Server error";
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine("Protocol error while trying to connect: {0}", ex.Message);
                return "Server error";
            }
            // authenticate
            if (smtpClient.Capabilities.HasFlag(SmtpCapabilities.Authentication))
            {
                try
                {
                    smtpClient.Authenticate(emailOptions.Sender, emailOptions.SenderPass);
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine("Invalid user name or password.");
                    return "Server error";
                }
                catch (SmtpCommandException ex)
                {
                    Console.WriteLine("Error trying to authenticate: {0}", ex.Message);
                    Console.WriteLine("\tStatusCode: {0}", ex.StatusCode);
                    return "Server error";
                }
                catch (SmtpProtocolException ex)
                {
                    Console.WriteLine("Protocol error while trying to authenticate: {0}", ex.Message);
                    return "Server error";
                }
            }
            //send
            try
            {
                smtpClient.Send(email);
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine("Error sending message: {0}", ex.Message);
                Console.WriteLine("\tStatusCode: {0}", ex.StatusCode);
                switch (ex.ErrorCode)
                {
                    case SmtpErrorCode.RecipientNotAccepted:
                        Console.WriteLine("\tRecipient not accepted: {0}", ex.Mailbox);
                        break;
                    case SmtpErrorCode.SenderNotAccepted:
                        Console.WriteLine("\tSender not accepted: {0}", ex.Mailbox);
                        break;
                    case SmtpErrorCode.MessageNotAccepted:
                        Console.WriteLine("\tMessage not accepted.");
                        break;
                }
                return "Invalid email";
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine("Protocol error while sending message: {0}", ex.Message);
                return "Server error";
            }

            smtpClient.Disconnect(true);
            return "Ok";
        }
    }
}
