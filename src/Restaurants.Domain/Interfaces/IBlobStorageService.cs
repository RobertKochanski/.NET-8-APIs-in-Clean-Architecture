namespace Restaurants.Domain.Interfaces
{
    public interface IBlobStorageService
    {
        string? GetBlobSASUrl(string? blobUrl);
        Task<string> UploadToBlobAsync(Stream data, string fileName);
    }
}
