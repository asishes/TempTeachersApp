using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherStatusCountDTO
    {
        public int Active { get; set; }
        public int New { get; set; }
        public int Pending { get; set; }
        public int HMApproved { get; set; }
    }
}
