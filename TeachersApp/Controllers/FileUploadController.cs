using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.DocumentDTO;
using TeachersApp.Entity.ModelDTO.PhotoDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.FileServices;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IUploadService _uploadService;
        private readonly TeachersAppDbcontext _context;

        public FileUploadController(IFileService fileService, IUploadService uploadService,TeachersAppDbcontext context)
        {
            _fileService = fileService;
            _uploadService = uploadService;
            _context = context;
        }

        [HttpPost("AddPhoto")]
        public async Task<IActionResult> AddPhoto([FromForm] UploadPhotoDTO photoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (photoDTO.PhotoFile != null && photoDTO.PhotoFile?.Length > 2 * 1024 * 1024)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "File size should not exceed 2 MB" });
                }

                var convertedPhoto = photoDTO.ToAddPhoto();
                if (photoDTO.PhotoFile != null && Request.Form.Files.Count > 0)
                {
                    string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                    string folderName = $"profile/Employee";
                    string createdImageName = await _fileService.SaveFileAsync(photoDTO.PhotoFile, allowedFileExtentions, folderName);
                    convertedPhoto.PhotoImageName = $"/Uploads/{folderName}/{createdImageName}";

                }

                var photo = await _uploadService.AddPhotosAsync(convertedPhoto);

                if (photo is null) return NotFound("Please check the fields are valid");

                return Ok(photo.GetPhoto());
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        [HttpPost("AddDocument")]
        public async Task<IActionResult> AddDocument([FromForm] AddDocumentDTO DocumentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                string[] allowedFileExtensions = { ".pdf", ".docx", ".jpeg", ".jpg" };
                string descriptionFolderName = $"Documents";

                // Attempt to save the file
                string createdFileName = await _fileService.SaveFileAsync(DocumentDTO.DocumentFile, allowedFileExtensions, descriptionFolderName);
                if (string.IsNullOrEmpty(createdFileName))
                {
                    return BadRequest(new { message = "File upload failed." });
                }

                var newDocument = DocumentDTO.ToAddDocument();
                newDocument.DocumentFileName = $"/Uploads/{descriptionFolderName}/{createdFileName}";

                // Attempt to create the homework in the repository
                var createdHomework = await _uploadService.AddDocumentsAsync(newDocument);
                if (createdHomework is null) return NotFound(new { message = "Please check the fields are valid" });

                return Ok(createdHomework.GetDocument());

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPatch("UpdateDocument/{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromForm] UpdateDocumentDTO updateDocumentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Fetch the existing document from the database
                var existingDocument = await _context.Documents.FindAsync(id);
                if (existingDocument == null)
                {
                    return NotFound(new { message = "Document not found" });
                }

                // Check if the document file is updated, and update its file path accordingly
                if (updateDocumentDTO.DocumentFile != null)
                {
                    string[] allowedFileExtensions = { ".pdf", ".docx", ".jpeg", ".jpg" };
                    string descriptionFolderName = "Documents";

                    // Save the file and get the created file name
                    string createdFileName = await _fileService.SaveFileAsync(updateDocumentDTO.DocumentFile, allowedFileExtensions, descriptionFolderName);
                    if (!string.IsNullOrEmpty(createdFileName))
                    {
                        existingDocument.DocumentFileName = $"/Uploads/{descriptionFolderName}/{createdFileName}";
                    }
                }

                // Update the document text if provided
                if (!string.IsNullOrEmpty(updateDocumentDTO.DocumentText))
                {
                    existingDocument.DocumentText = updateDocumentDTO.DocumentText;
                }

                // Map the DTO to the existing document (including the file path and document text updates)
                existingDocument = updateDocumentDTO.ToUpdateDocument(existingDocument);

                // Attempt to update the document in the database
                var updatedDoc = await _uploadService.UpdateDocumentAsync(id, existingDocument);
                if (updatedDoc is null)
                {
                    return NotFound(new { message = "Please check the fields are valid" });
                }

                return Ok(updatedDoc.GetDocument());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("UpdatePhoto/{id}")]
        public async Task<IActionResult> UpdatePhoto(int id, [FromForm] UpdatePhotoDTO updatePhotoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Fetch the existing document from the database
                var existingPhoto = await _context.Photos.FindAsync(id);
                if (existingPhoto == null)
                {
                    return NotFound(new { message = "Photo not found" });
                }

                
                    if (updatePhotoDTO.PhotoFile != null && updatePhotoDTO.PhotoFile?.Length > 2 * 1024 * 1024)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { message = "File size should not exceed 2 MB" });
                    }

                    var convertedPhoto = updatePhotoDTO.ToUpdatePhoto(existingPhoto);
                    if (updatePhotoDTO.PhotoFile != null && Request.Form.Files.Count > 0)
                    {
                        string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                        string folderName = $"profile/Employee";
                        string createdImageName = await _fileService.SaveFileAsync(updatePhotoDTO.PhotoFile, allowedFileExtentions, folderName);
                        convertedPhoto.PhotoImageName = $"/Uploads/{folderName}/{createdImageName}";

                    }

                    var photo = await _uploadService.UpdatePhotoAsync(id,convertedPhoto);

                    if (photo is null) return NotFound("Please check the fields are valid");

                    return Ok(photo.GetPhoto());
                }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
