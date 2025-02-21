using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.DocumentDTO;
using TeachersApp.Entity.ModelDTO.SchoolClassDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.ModelDTO.UserDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class UserMapper
    {

        public static User ToUpdateUser(this UpdateUserDTO updateUserDTO, User existingUser)
        {
            if (updateUserDTO.FirstName != null)
            {
                existingUser.FirstName = updateUserDTO.FirstName;
            }

            if (updateUserDTO.LastName != null)
            {
                existingUser.LastName = updateUserDTO.LastName;
            }

            if (updateUserDTO.DateOfBirth != default(DateTime))
            {
                existingUser.DateOfBirth = updateUserDTO.DateOfBirth;
            }

            return existingUser;
        }

    



    public static GetUserDTO ToGetUserDTO(this User user)
        {
            return new GetUserDTO
            {
                UserID = user.UserID,
                Username = user.Username ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                DateOfBirth = user.DateOfBirth ?? (DateTime?) null,  
                RoleID = user.RoleID,
                RoleName = user.Role?.RoleName ?? string.Empty,
                EmployeeID = user.EmployeeID ?? null,  
                SchoolID = user.Employee?.SchoolID ?? null, 
                EmployeeTypeID = user.Employee?.EmployeeTypeID ?? null,
                EmployeeTypeName = user.Employee != null && user.Employee.EmployeeType != null
                ? user.Employee.EmployeeType.Employeetype: string.Empty,
                EmployeeName = $"{user.FirstName ?? string.Empty} {user.LastName ?? string.Empty}".Trim()

            };
        }
    }
}
