using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherDocumentDTO
    {
        public int TeacherID { get; set; }

        public string DocumentName { get; set; } = null!;

        public IFormFile DocumentPath { get; set; } = null!;
    }
}
