using System.Net;
using System.Net.Mail;

namespace UploadNotificationFunction.Services
{
	public class EmailService: IEmailService
	{        
        public void SendLetter(MailMessage message, NetworkCredential credential)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;
            smtpClient.Send(message);
        }
    }
}

