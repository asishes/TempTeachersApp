using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.UserDTO
{
    public class GetUserDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public int? RoleID { get; set; }

        public string RoleName { get; set; } = string.Empty!;

        public int? EmployeeID { get; set; } 

        public int? SchoolID { get; set; }

        public int? EmployeeTypeID { get; set; }

        public string EmployeeTypeName { get; set; } = string.Empty!;

        public string EmployeeName { get; set; } = string.Empty!;


    }
}
