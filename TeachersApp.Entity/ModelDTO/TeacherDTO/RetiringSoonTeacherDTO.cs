using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class RetiringSoonTeacherDTO
    {
        public int TeacherID { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public DateTime? HireDate { get; set; }

        public DateTime? RetirementDate { get; set; }

        public int? DesignationID { get; set; }

        public int? SchoolID { get; set; }

        public int? StatusID { get; set; }

        public int? SupervisorID { get; set; }
    }
}
