using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.DocumentDTO;
using TeachersApp.Entity.ModelDTO.PhotoDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class UploadMapper
    {
        public static GetUploadPhotoDTO GetPhoto(this Photo getPhoto)
        {
            return new GetUploadPhotoDTO
            {
                PhotoID = getPhoto.PhotoID,
                PhotoImageName = getPhoto.PhotoImageName,
            };
        }

        public static Photo ToAddPhoto(this UploadPhotoDTO uploadPhoto)
        {
            return new Photo
            {
                PhotoFile = uploadPhoto.PhotoFile // Correct file name extraction

            };

        }

        public static Photo ToUpdatePhoto(this UpdatePhotoDTO updatePhotoDTO, Photo existingPhoto)
        {


            if (updatePhotoDTO.PhotoFile != null)
            {
                existingPhoto.PhotoFile = updatePhotoDTO.PhotoFile;
            }

            return existingPhoto;
        }

        public static GetDocumentDTO GetDocument(this Document getDocument)
        {
            return new GetDocumentDTO
            {
                DocumentID = getDocument.DocumentID,
                DocumentName = getDocument.DocumentFileName,
            };
        }

        public static Document ToAddDocument(this AddDocumentDTO uploadDocument)
        {
            return new Document
            {
                DocumentText = uploadDocument.DocumentName,
                DocumentFile = uploadDocument.DocumentFile

            };

        }

        public static Document ToUpdateDocument(this UpdateDocumentDTO updateDocumentDTO, Document existingDocument)
        {
            if (!string.IsNullOrEmpty(updateDocumentDTO.DocumentText))
            {
                existingDocument.DocumentText = updateDocumentDTO.DocumentText;
            }

            if (updateDocumentDTO.DocumentFile != null)
            {
                existingDocument.DocumentFile = updateDocumentDTO.DocumentFile;
            }

            return existingDocument;
        }
      


    }
}
