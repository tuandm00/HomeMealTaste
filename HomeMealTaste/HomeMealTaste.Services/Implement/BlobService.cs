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
    }
}
