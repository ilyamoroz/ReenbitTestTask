using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using System.Net.Mail;
using UploadNotificationFunction;
using UploadNotificationFunction.Services;

namespace ReenbitTestTask.Tests
{
    public  class UploadNotificationTests
    {
        private Mock<ILogger> _loggerMock;
        private Mock<ISASService> _iSASServiceMock;
        private Mock<IEmailService> _emailServiceMock;
        private UploadNotification _function;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger>();
            _iSASServiceMock = new Mock<ISASService>();
            _emailServiceMock = new Mock<IEmailService>();
            _configurationMock = new Mock<IConfiguration>();
            _function = new UploadNotification(_emailServiceMock.Object, _iSASServiceMock.Object,  _configurationMock.Object);
        }


        [Test]
        public void Run_ValidBlobName_CallsSendLetter()
        {
            var name = "user@example.com/file.txt";
            var blobMock = new Mock<CloudBlob>();
            blobMock.Setup(b => b.Uri).Returns(new System.Uri("http://example.com/file.txt"));
            _iSASServiceMock.Setup(s => s.GetBlobSASUrl(blobMock.Object)).Returns("http://example.com/sas");

            _function.Run(name, _loggerMock.Object);

            _emailServiceMock.Verify(e => e.SendLetter(
                It.Is<MailMessage>(m =>
                    m.From.Address == "your_sender_email@example.com" &&
                    m.To[0].Address == "user@example.com" &&
                    m.Subject == "Subject" &&
                    m.Body.Contains("Your file upload successfully! Link on file: http://example.com/sas")),
                It.IsAny<System.Net.NetworkCredential>()), Times.Once);
        }


        [Test]
        public void Run_NullBlob_DoesNotCallSendLetter()
        {
            var name = "user@example.com/file.txt";
            CloudBlob nullBlob = null;

            _function.Run(name, _loggerMock.Object);

            _emailServiceMock.Verify(e => e.SendLetter(It.IsAny<MailMessage>(), It.IsAny<System.Net.NetworkCredential>()), Times.Never);
        }
    }
}
