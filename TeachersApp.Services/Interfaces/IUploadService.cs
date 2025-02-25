using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface IUploadService
    {
        Task<Photo> AddPhotosAsync(Photo photo);

        Task<Photo> UpdatePhotoAsync(int photoId, Photo photo);


        Task<Document> AddDocumentsAsync(Document document);

        Task<Document> UpdateDocumentAsync(int documentId,Document document);

    }
}
