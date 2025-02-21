using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolClassDTO
{
    public class UpdateDivisionAndStudentCountDTO
    {
        public string Division { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}
