using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace UploadNotificationFunction.Services
{
    public class SASService : ISASService
    {
        public string GetBlobSASUrl(CloudBlob blob)
        {
            var expiryTime = DateTimeOffset.UtcNow.AddHours(1);

            var sasPolicy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = expiryTime
            };

            string sasToken = blob.GetSharedAccessSignature(sasPolicy);

            string sasUri = blob.Uri + sasToken;

            return sasUri;
        }
    }
}
