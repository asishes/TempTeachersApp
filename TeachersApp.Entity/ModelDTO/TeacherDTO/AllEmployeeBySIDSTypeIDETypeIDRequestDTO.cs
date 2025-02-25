using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class AllEmployeeBySIDSTypeIDETypeIDRequestDTO
    {
        public int? SchoolID { get; set; }
        public List<int?> SchoolTypeIDs { get; set; } = new List<int?>();
        public List<int?> EmployeeTypeIDs { get; set; } = new List<int?>();
    }
}
