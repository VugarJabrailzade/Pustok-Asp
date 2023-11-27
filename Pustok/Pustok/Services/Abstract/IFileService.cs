using Microsoft.AspNetCore.Http;
using Pustok.Contracts;

namespace Pustok.Services.Abstract
{
    public interface IFileService
    {
        string Upload(IFormFile file, string path);

        string Upload(IFormFile file, UploadDirectory directoryPath);
        void Delete(string path);

        void Delete(UploadDirectory directory, string fileName);
    }
}
