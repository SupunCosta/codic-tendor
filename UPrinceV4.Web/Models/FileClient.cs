using System;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Models;

public class FileClient
{
    public string PersistPhoto(string saveAsFileName, ITenantProvider tenantProvider, IFormFile icon)
    {
        //string connectionString = "DefaultEndpointsProtocol=https;AccountName=uprincev4dev;AccountKey=rZrNDwBXL0cKSkf7JD0b6/+NBhafqS3GPNOHN4UyeZ0aoy7MMgTV1fAm4tmo0rmDBHreZReZx165R0LZ0yf5DA==;EndpointSuffix=core.windows.net";
        //string containerName = "uprincev4dev";
        var container = new BlobContainerClient(tenantProvider.GetTenant().StorageConnectionString,
            tenantProvider.GetTenant().AzureContainer);

        try
        {
            // Get a reference to a blob
            var blob = container.GetBlobClient(saveAsFileName);

            // Open the file and upload its data
            using (var file = icon.OpenReadStream())
            {
               // blob.DeleteIfExists();
                blob.Upload(file);
            }

            var uri = blob.Uri.AbsoluteUri;
            return uri;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string PersistPhotoInNewFolder(string saveAsFileName, ITenantProvider tenantProvider, IFormFile icon,
        string folderName)
    {
        //string connectionString = "DefaultEndpointsProtocol=https;AccountName=uprincev4dev;AccountKey=rZrNDwBXL0cKSkf7JD0b6/+NBhafqS3GPNOHN4UyeZ0aoy7MMgTV1fAm4tmo0rmDBHreZReZx165R0LZ0yf5DA==;EndpointSuffix=core.windows.net";
        //string containerName = "uprincev4dev";
        var container = new BlobContainerClient(tenantProvider.GetTenant().StorageConnectionString,
            tenantProvider.GetTenant().AzureContainer);

        try
        {
            // Get a reference to a blob
            var path = saveAsFileName;
            if (!string.IsNullOrEmpty(folderName)) path = folderName + "/" + DateTime.UtcNow + saveAsFileName;

            var blob = container.GetBlobClient(path);

            // Open the file and upload its data
            using (var file = icon.OpenReadStream())
            {
                //blob.DeleteIfExists();
                blob.Upload(file);
            }

            var uri = blob.Uri.AbsoluteUri;
            return uri;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string PersistThZipUpload(string saveAsFileName, ITenantProvider tenantProvider, IFormFile icon,
        string Project, string Product, string existFileName)
    {
        var container = new BlobContainerClient(tenantProvider.GetTenant().StorageConnectionString,
            tenantProvider.GetTenant().AzureContainer);

        try
        {
            // Get a reference to a blob
            var path = saveAsFileName;
            if (!string.IsNullOrEmpty(Product)) path = Project + "/" + Product + "/" + saveAsFileName;

            var path2 = existFileName;

            if (existFileName != null)
            {
                path2 = Project + "/" + Product + "/" + existFileName;

                var hh = container.GetBlobClient(path2);

                hh.Delete();
            }

            var blob = container.GetBlobClient(path);


            // Open the file and upload its data
            using (var file = icon.OpenReadStream())
            {
                //blob.DeleteIfExists();
                blob.Upload(file);
            }

            var uri = blob.Uri.AbsoluteUri;
            return uri;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string PersistLotUpload(string fileName, ITenantProvider tenantProvider, IFormFile file,
        string lotId)
    {
        //string connectionString = "DefaultEndpointsProtocol=https;AccountName=uprincev4dev;AccountKey=rZrNDwBXL0cKSkf7JD0b6/+NBhafqS3GPNOHN4UyeZ0aoy7MMgTV1fAm4tmo0rmDBHreZReZx165R0LZ0yf5DA==;EndpointSuffix=core.windows.net";
        //string containerName = "uprincev4dev";
        var container = new BlobContainerClient(tenantProvider.GetTenant().StorageConnectionString,
            tenantProvider.GetTenant().AzureContainer);

        try
        {
            // Get a reference to a blob
            var path = lotId + "/" + file.FileName;
            if (!string.IsNullOrEmpty(lotId)) path = "LotDocuments" + "/" + path + "/" + "Second";

            var blob = container.GetBlobClient(path);

            // Open the file and upload its data
            using (var lotFile = file.OpenReadStream())
            {
                //blob.DeleteIfExists();
                blob.Upload(lotFile);
            }

            var uri = blob.Uri.AbsoluteUri;
            return uri;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public string PersistLotDocUpload(string fileName, ITenantProvider tenantProvider, IFormFile file,
        string lotId,string documentCategory)
    {
        //string connectionString = "DefaultEndpointsProtocol=https;AccountName=uprincev4dev;AccountKey=rZrNDwBXL0cKSkf7JD0b6/+NBhafqS3GPNOHN4UyeZ0aoy7MMgTV1fAm4tmo0rmDBHreZReZx165R0LZ0yf5DA==;EndpointSuffix=core.windows.net";
        //string containerName = "uprincev4dev";
        var container = new BlobContainerClient(tenantProvider.GetTenant().StorageConnectionString,
            tenantProvider.GetTenant().AzureContainer);

        try
        {
            // Get a reference to a blob
            var path = lotId + "/" + documentCategory +"/" + file.FileName;
            if (!string.IsNullOrEmpty(lotId)) path = "LotDocuments"+ "/" + path ;

            var blob = container.GetBlobClient(path);

            // Open the file and upload its data
            using (var lotFile = file.OpenReadStream())
            {
                //blob.DeleteIfExists();
                blob.Upload(lotFile);
            }

            var uri = blob.Uri.AbsoluteUri;
            return uri;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
//public void PersistPhoto(IFormFile fileToPersist, string saveAsFileName)
//{
//    string connectionString = "Azure Storage Connection String";
//    string containerName = "Azure Storage Container Name";
//    BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

//    try
//    {
//        // Get a reference to a blob
//        BlobClient blob = container.GetBlobClient(saveAsFileName);

//        // Open the file and upload its data
//        using (Stream file = fileToPersist.OpenReadStream())
//        {
//            blob.Upload(file);
//        }

//        uri = blob.Uri.AbsoluteUri;
//    }
//    catch
//    {
//        // log error
//    }
//}