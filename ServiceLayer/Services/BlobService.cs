using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public string GetUri(string name)
        {
            var client = _blobServiceClient.GetBlobContainerClient("twitterapi");

            var blob = client.GetBlobClient(name);

            return blob.Uri.AbsoluteUri;
        }

        public async Task UploadBlobAsync(Stream content, string name, string contentType)
        {
            var client = _blobServiceClient.GetBlobContainerClient("twitterapi");

            var blob = client.GetBlobClient(name);

            await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
        }

        public async Task DeleteBlobAsync(string name)
        {
            var client = _blobServiceClient.GetBlobContainerClient("twitterapi");

            var blob = client.GetBlobClient(name);

            await blob.DeleteIfExistsAsync();
        }
    }
}
