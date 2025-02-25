using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherHistoryDTO
{
    public class CreateTeacherHistoryDTO
    {
        public int EmployeeID { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ChangedByID { get; set; }
        public string? ChangeDescription { get; set; }
        public int ChangeTypeID { get; set; }
        public int ChangeFromSchoolID { get; set; }
        public int ChangeToSchoolID { get; set; }
        public int PromotedFromPositionID { get; set; }
        public int PromotedToPositionID { get; set; }
    }
}
