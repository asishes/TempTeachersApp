using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.DocumentDTO
{
    public class UpdateDocumentDTO
    {
        public string? DocumentText { get; set; }
        public IFormFile? DocumentFile { get; set; }
    }
}
