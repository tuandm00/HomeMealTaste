using Azure.Storage.Blobs;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadQuestImgAndReturnImgPathAsync(IFormFile file, string containerName)
        {
            if (file == null) return null!;

            var renameFile = file.FileName.Replace(file.FileName, containerName);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}_{renameFile}");
            await blobClient.UploadAsync(file.OpenReadStream(), true);

            return blobClient.Uri.AbsoluteUri;
        }


        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=homemealtaste;AccountKey=xBT5OBqwV85Z3gHHxPBPTlabsmEvGMtoJUrKhcmNiqurBcapv3EGD6gvSS6GjYhsnJUKv3iBD8io+ASt17IZQA==;EndpointSuffix=core.windows.net";
        private const string containerName = "meal-image"; // Replace with your container name

        private string GetLocalFilePathFromUri(string fileUri)
        {
            Uri uri = new Uri(fileUri);
            return uri.LocalPath;
        }
        public async Task<string> UploadImage(string imageData)
        {
            try
            {
                string localFilePath = GetLocalFilePathFromUri(imageData);
                byte[] fileBytes = File.ReadAllBytes(localFilePath);
                string blobName = "image_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                using (MemoryStream stream = new MemoryStream(fileBytes))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Save metadata to the database
                // Code to save metadata to Azure SQL Database can be implemented here

                Console.WriteLine("Image uploaded successfully!");
                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }

    }
}
