using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IFileSaver
{
}

public class AzureBlobStorageFileSaver : IFileSaver
{
    public AzureBlobStorageFileSaver(string connectionString)
    {
    }

    public void PersistPhoto(IFormFile fileToPersist, string saveAsFileName)
    {
        var connectionString = "Azure Storage Connection String";
        var containerName = "Azure Storage Container Name";
        var container = new BlobContainerClient(connectionString, containerName);

        try
        {
            // Get a reference to a blob
            var blob = container.GetBlobClient(saveAsFileName);

            // Open the file and upload its data
            using (var file = fileToPersist.OpenReadStream())
            {
                blob.Upload(file);
            }

            var uri = blob.Uri.AbsoluteUri;
        }
        catch
        {
            // log error
        }
    }
}