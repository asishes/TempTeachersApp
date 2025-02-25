using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolTypeDTO
{
    public class AddSchoolTypeDTO
    {

        public string SchoolTypeName { get; set; } = string.Empty;

        public int Class { get; set; }
    }
}
