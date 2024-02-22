namespace webapi.Interfaces
{
    public interface IBlobStorageService
    {
        Task UploadFileToBlobStorage(IFormFile file, string email);
    }
}
