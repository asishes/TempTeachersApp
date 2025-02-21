using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.PositionDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class SchoolPositionMapper
    {

        public static SchoolPosition AddSchoolPositionDTO(this CreateNewPositionDTO positionDTO)
        {
            return new SchoolPosition
            {
                DesignationID = positionDTO.DesignationID,
                SubjectID = positionDTO.SubjectID,
                SchoolID = positionDTO.SchoolID,
            };
        }
 
        public static GetVacantAndNewSchoolPositionsDTO ToGetVacantAndNewSchoolPositionsDTO ( this SchoolPosition schoolPosition )
        {
            return new GetVacantAndNewSchoolPositionsDTO
            {
                SchoolPositionID = schoolPosition.PositionID,
                SchoolID = schoolPosition.SchoolID,
                SchoolName = schoolPosition.School.SchoolName ?? "No School",
                Address = schoolPosition.School.Address +" "+ schoolPosition.School.City.CityName ?? "No City",
                SubjectID = schoolPosition.SubjectID,
                SubjectName = schoolPosition.Subject.SubjectText ?? "No Subject",
                DesignationID = schoolPosition.DesignationID,
                DesignationName = schoolPosition.Designation.DesignationText ?? "No Designation",
                StatusID = schoolPosition.StatusID, 
                Status = schoolPosition.Status.StatusText ?? "No Status"

            };
        }
    }
}
