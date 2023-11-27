using Microsoft.AspNetCore.Http;
using Pustok.Contracts;
using Pustok.Extensions;
using Pustok.Services.Abstract;
using System;
using System.IO;

namespace Pustok.Services.Concretes
{
    public class FileService : IFileService
    {
        public string Upload(IFormFile file, string path)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var uploadPath = Path.Combine(path,uniqueFileName);
            using FileStream fileStream = new FileStream(uploadPath, FileMode.Create);
            file.CopyTo(fileStream);

            return uniqueFileName;

        }
            

        public string Upload(IFormFile file, UploadDirectory directoryPath)
        {
            var uniqueFileName = GetUniqueFileName(file.FileName);
            var uploadPath = directoryPath.GetAbsolutePath(uniqueFileName);
            using FileStream fileStream = new FileStream(uploadPath, FileMode.Create);
              //Copyto ramdan birbahsa sisteme yazir) file stream ile fayli ramdan oturub diskde saxlamag; temin edirik
            file.CopyTo(fileStream);

            return uniqueFileName;
        }


        public void Delete (string path)
        {
            File.Delete(path);
        }

        public void Delete(UploadDirectory directory, string fileName)
        {
            var asbolutePath = directory.GetAbsolutePath(fileName);
            Delete(asbolutePath);
        }

        private string GetUniqueFileName(string fileName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        }
    }
}
