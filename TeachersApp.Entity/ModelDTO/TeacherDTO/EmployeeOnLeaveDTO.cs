using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class EmployeeOnLeaveDTO
    {
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int CompletedDays { get; set; }
        public int RemainingDays { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        public TeacherPopUpDTO TeacherPopUpDTO { get; set; } = new TeacherPopUpDTO();
    }
}
