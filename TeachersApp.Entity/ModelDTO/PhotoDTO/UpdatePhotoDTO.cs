using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PhotoDTO
{
    public class UpdatePhotoDTO
    {
        public IFormFile? PhotoFile { get; set; }
    }
}
