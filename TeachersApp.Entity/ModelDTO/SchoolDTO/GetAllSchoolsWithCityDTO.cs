using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolDTO
{
    public class GetAllSchoolsWithCityDTO
    {
        public int SchoolID { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public int CityID { get; set; }

        public string CityName { get; set; } = string.Empty;
    }
}
