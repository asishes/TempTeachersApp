using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.UserDTO.ForgotPasswordDTO;

namespace TeachersApp.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO);

        Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);

        

    }
}
