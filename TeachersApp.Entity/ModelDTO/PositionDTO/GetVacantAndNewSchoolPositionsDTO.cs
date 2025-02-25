using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.PositionDTO
{
    public class GetVacantAndNewSchoolPositionsDTO
    {
        public int SchoolPositionID { get; set; }

        public int SchoolID { get; set; }
        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int SubjectID { get; set; }

        public string SubjectName { get; set; } = string.Empty;
        public int DesignationID { get; set; }

        public string DesignationName { get; set; } = string.Empty;

        public int StatusID { get; set; }

        public string Status { get; set; } = string.Empty;


    }
}
