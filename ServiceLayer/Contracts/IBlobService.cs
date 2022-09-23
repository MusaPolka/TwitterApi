using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IBlobService
    {
        string GetUri(string name);
        Task UploadBlobAsync(Stream content, string name, string contentType);
        Task DeleteBlobAsync(string name);
    }
}
