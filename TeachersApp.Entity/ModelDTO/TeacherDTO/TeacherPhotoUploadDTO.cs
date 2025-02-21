using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherPhotoUploadDTO
    {
        [Required]
        public int TeacherID { get; set; }

        [Required]
        public IFormFile PhotoFile { get; set; } = null!;
    }
}
