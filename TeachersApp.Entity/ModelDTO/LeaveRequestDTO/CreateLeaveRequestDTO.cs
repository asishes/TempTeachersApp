using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.LeaveRequestDTO
{
    public class CreateLeaveRequestDTO
    {
        public int EmployeeID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? RequestorComment { get; set; }
        public int? DocumentID { get; set; }
    }
}
