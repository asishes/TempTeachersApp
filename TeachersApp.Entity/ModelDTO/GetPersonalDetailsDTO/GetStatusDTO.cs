using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO
{
    public class GetStatusDTO
    {
        public int StatusID { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public string StatusType { get; set; } = string.Empty;


    }
}
