using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Helpers
{
    public class EmailConfiguration
    {
        public string? UserMail { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string? FromMail { get; set; }
    }
}
