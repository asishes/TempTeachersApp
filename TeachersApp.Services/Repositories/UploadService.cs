using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Services.Repositories
{
    public class UploadService : IUploadService
    {
        private readonly TeachersAppDbcontext _context;

        public UploadService(TeachersAppDbcontext context)
        {
            _context = context;
        }

        #region TeacherPhotoUpload

        public async Task<Photo> AddPhotosAsync(Photo photo)
        {


            await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync();

            return photo;
        }


        #endregion

        public async Task<Photo> UpdatePhotoAsync(int photoId, Photo photo)
        {
            var existingPhoto = await _context.Photos.FindAsync(photoId);
            if (existingPhoto == null)
            {
                return null;
            }

            _context.Photos.Update(photo); // Update the document in the database
            await _context.SaveChangesAsync();
            return photo;
        }


        #region TeacherDocumentUpload

        public async Task<Document> AddDocumentsAsync(Document document)
        {
            document.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "Pending" && s.StatusType == "Document")
            .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();

            return document;
        }


        #endregion


        public async Task<Document> UpdateDocumentAsync(int documentId, Document document)
        {
            var existingDocument = await _context.Documents.FindAsync(documentId);
            if (existingDocument == null)
            {
                return null;
            }

            _context.Documents.Update(document); // Update the document in the database
            await _context.SaveChangesAsync();
            return document;
        }





    }
}
