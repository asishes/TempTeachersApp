using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void DeleteFile(string FileNameWithExtension)
        {
            if (string.IsNullOrEmpty(FileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(FileNameWithExtension));
            }
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads", FileNameWithExtension);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file path");
            }
            File.Delete(path);
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string folderName)
        {
            //if (imageFile == null)
            //{
            //    throw new ArgumentNullException(nameof(imageFile));
            //}
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads/{folderName}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var ext = Path.GetExtension(imageFile.FileName);
            if (!allowedFileExtensions.Contains(ext))
            {
                return null;
                //throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed");
            }

            var fileName = $"{imageFile.FileName}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            return fileName;
        }

        public async Task<Boolean> CheckFileExists(string FilePath)
        {
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads/{FilePath}");
            if (!Directory.Exists(path))
            {
                return false;
            }
            return true;
        }
    }
}

