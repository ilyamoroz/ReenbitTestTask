using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using UploadNotificationFunction.Services;

namespace UploadNotificationFunction
{
    [StorageAccount("BlobConnectionString")]
    public class UploadNotification
    {
        private readonly IEmailService _emailService;
        private readonly ISASService _isaSService;
        private readonly IConfiguration _configuration;

        private readonly string _storageConnectionString;
        private readonly string _containerName;
        private readonly string _senderEmail;
        private string _senderPassword;

        public UploadNotification(IEmailService emailService, ISASService isaSService, IConfiguration configuration)
        {
            _emailService = emailService ?? throw new ArgumentNullException();
            _isaSService = isaSService ?? throw new ArgumentNullException();
            _configuration = configuration ?? throw new ArgumentNullException();
            _storageConnectionString = _configuration["StorageConnectionString"];
            _containerName = _configuration["StorageContainerName"];
            _senderEmail = _configuration["SenderEmail"];
            _senderEmail = _configuration["SenderPassword"];
        }


        [FunctionName("UploadNotification")]
        public void Run([BlobTrigger("filestorage/{name}")]string name, ILogger log)
        {
            var userEmail = name.Split('/')[0];

            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_containerName);
            var blob = container.GetBlobReference(name);

            var fileUrl = _isaSService.GetBlobSASUrl(blob);

            _emailService.SendLetter(
                message: new MailMessage(_senderEmail, userEmail, "Subject", $"Your file upload successfully! Link on file: {fileUrl}"), 
                credential: new NetworkCredential(_senderEmail, _senderPassword));
        }
    }
}

