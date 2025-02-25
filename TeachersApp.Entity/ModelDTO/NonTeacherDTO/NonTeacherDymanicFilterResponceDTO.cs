using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.NonTeacherDTO
{
    public class NonTeacherDymanicFilterResponceDTO
    {
        public int? RetiringInMonths { get; set; }
        public int? SchoolId { get; set; }
        public string? UniqueId { get; set; }
        public int? MinExperienceYear { get; set; }
        public int? MaxExperienceYear { get; set; }
        public List<string>? Statuses { get; set; }
        public int? AssignedSchoolId { get; set; }
    }
}
