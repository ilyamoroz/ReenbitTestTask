using System;
using System.Net;
using System.Net.Mail;
namespace UploadNotificationFunction.Services
{
	public interface IEmailService
	{
		void SendLetter(MailMessage message, NetworkCredential credential);
	}
}

