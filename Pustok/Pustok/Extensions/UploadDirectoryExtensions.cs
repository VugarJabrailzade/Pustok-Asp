using Pustok.Contracts;
using Pustok.Exceptions;
using System.IO;
using System.Linq;

namespace Pustok.Extensions
{
    public static class UploadDirectoryExtensions
    {
        public static string GetAbsolutePath(this UploadDirectory uploadDirectory)
        {
            switch (uploadDirectory)
            {
                case UploadDirectory.Products:
                    return @"C:\Users\vcebr\Pustok\Pustok\Pustok\wwwroot\images\product\";
                default:
                    throw new UploadDirectoryException("Upload Path Not Found!", uploadDirectory);
            }
        }

        public static string GetUrl(this UploadDirectory uploadDir)
        {
            switch (uploadDir)
            {
                case UploadDirectory.Products:
                    return "/images/product";
                default:
                    throw new UploadDirectoryException("Upload path not found", uploadDir);
            }
        }

        public static string GetAbsolutePath(this UploadDirectory uploadDirectory, string fileName)
        {
            return Path.Combine(GetAbsolutePath(uploadDirectory), fileName);
        }

        public static string GetUrl(this UploadDirectory uploadDir, string fileName)
        {
            return $"{GetUrl(uploadDir)}/{fileName}";
        }

    }
}
