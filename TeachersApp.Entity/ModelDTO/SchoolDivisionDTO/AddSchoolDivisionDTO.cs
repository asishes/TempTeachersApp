using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolDivisionDTO
{
    public class AddSchoolDivisionDTO
    {
        public int SchoolClassID { get; set; }
 
        public string Division { get; set; }
        public int StudentCount { get; set; }
    }
}
