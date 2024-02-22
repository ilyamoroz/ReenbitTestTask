using Microsoft.WindowsAzure.Storage.Blob;

namespace UploadNotificationFunction.Services
{
    public interface ISASService
    {
        string GetBlobSASUrl(CloudBlob blob);
    }
}
