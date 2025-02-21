using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.DocumentDTO
{
    public class AddDocumentDTO
    {
        [Required]
        public string? DocumentName { get; set; }
        [Required]
        public IFormFile DocumentFile { get; set; } = null!;
    }
}
