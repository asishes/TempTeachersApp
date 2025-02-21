using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, bool rememberme);
      
    }
}
