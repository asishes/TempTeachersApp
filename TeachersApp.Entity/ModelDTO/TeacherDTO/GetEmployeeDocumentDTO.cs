using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class GetEmployeeDocumentDTO
    {
        public int? DocumentID { get; set; }

        public string? DocumentName { get; set; }
        public string? Documentpath { get; set; }
    }
}
