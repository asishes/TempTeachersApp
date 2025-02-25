using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Services.FileServices
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string folderName);
        void DeleteFile(string FileNameWithExtension);
        Task<Boolean> CheckFileExists(string FilePath);
    }
}
