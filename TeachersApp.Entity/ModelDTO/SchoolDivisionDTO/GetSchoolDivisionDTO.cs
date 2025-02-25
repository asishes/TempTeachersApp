using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolDivisionDTO
{
    public class GetSchoolDivisionDTO
    {
        public int SchoolDivisionID { get; set; }
        public int SchoolID { get; set; }
        public string Division { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}
