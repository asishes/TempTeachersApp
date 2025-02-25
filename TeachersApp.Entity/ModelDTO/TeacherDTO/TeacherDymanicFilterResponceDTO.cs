using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherDymanicFilterResponceDTO
    {
        public int? SubjectId { get; set; }
        public int? RetiringInMonths { get; set; }
        public int? SchoolId { get; set; }
        public string? UniqueId { get; set; }
        public int? MinExperienceYear { get; set; }
        public int? MaxExperienceYear { get; set; }
        public List<string>? Statuses { get; set; }
        public int? AssignedSchoolId { get; set; }
    }
}

