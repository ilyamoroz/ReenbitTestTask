using Azure.Storage.Blobs;
using webapi.Interfaces;

namespace webapi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private string _connectionString;
        private string _containerName;

        private readonly IConfiguration _configuration;
        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException();
            _connectionString = _configuration["StorageConnectionString"];
            _containerName = _configuration["StorageContainerName"];
        }


        public async Task UploadFileToBlobStorage(IFormFile file, string email)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var filePath = $"{email}/{file.FileName}";

            var blobClient = containerClient.GetBlobClient(filePath);

            using (Stream stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}
