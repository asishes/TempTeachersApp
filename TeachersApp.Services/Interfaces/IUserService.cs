using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.UserDTO;
using TeachersApp.Entity.ModelDTO.UserDTO.ResetPasswordDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDTO dto);

        Task<string> AuthenticateAsync(LoginDTO loginDTO);
   
         Task<User?> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto);
        
        Task<User?> GetUserByTokenAsync(string token);
        Task<User?> GetUserByUserIDAsync(int userID);
        Task<bool> ResetPasswordByUserAsync(ResetPasswordByUserDTO resetPasswordDto, string token);
    }
}
