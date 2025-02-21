using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class PersonalServiceMapper
    {
        public static GetSchoolDTO GetSchoolDTO(this School schoolDTO)
        {

            return new GetSchoolDTO
            {
               SchoolId = schoolDTO.SchoolID,
               SchoolName = schoolDTO.SchoolName ?? string.Empty,
               City = schoolDTO.City.CityName ?? string.Empty,
            };
        }

        public static GetSubjectDTO ToGetSubjectDTO(this Subject subject)
        {
            return new GetSubjectDTO
            {
                SubjectID = subject.SubjectID,
                SubjectName = subject.SubjectText,
            };
        }

        public static GetStatusDTO ToGetStatusDTO(this Status status)
        {
            return new GetStatusDTO
            {
                StatusID = status.StatusID,
                StatusName = status.StatusText,
                StatusType = status.StatusType,
            };
        }

        public static GetReligionDTO ToGetEmployeeReligionDTO(this EmployeeReligion religion)
        {
            return new GetReligionDTO
            {
                ReligionID = religion.ReligionID,
                ReligionName = religion.ReligionName,
            };
        }

        public static GetMaritalStatusDTO ToGetEmployeeMaritalStatusDTO(this EmployeeMaritalStatus maritalStatus)
        {
            return new GetMaritalStatusDTO
            {
                MaritalStatusID = maritalStatus.MaritalStatusID,
                MaritalStatusName = maritalStatus.MaritalStatusName,
            };
        }

        public static GetGenderDTO ToGetEmployeeGenderDTO(this EmployeeSex gender)
        {
            return new GetGenderDTO
            {
                GenderID = gender.SexID,
                GenderName = gender.Sex
            };
        }

        public static GetEmployeeTypeDTO ToGetEmployeeTypeDTO(this EmployeeType employeeType)
        {
            return new GetEmployeeTypeDTO
            {
                EmployeeTypeID = employeeType.EmployeeTypeID,
                EmployeeTypeName = employeeType.Employeetype,
            };
        }

        public static GetEmployeeDTO ToGetEmployeePersonalDTO(this Employee employee)
        {
            return new GetEmployeeDTO
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = employee.FirstName+" "+ employee.LastName,
            };
        }

        public static GetEmployeeCategoryDTO ToGetEmployeeCategoryDTO(this EmployeeCategory category)
        {
            return new GetEmployeeCategoryDTO
            {
                EmployeeCategoryId = category.CategoryID,
                EmployeeCategoryName = category.CategoryName,
            };
        }

        public static GetDistrictDTO ToGetDistrictDTO(this District district)
        {
            return new GetDistrictDTO
            {
                DistrictID = district.DistrictID,
                DistrictName = district.DistrictName,
            };
        }

        public static GetDesignationDTO ToGetDesignationDTO(this Designation designation)
        {
            return new GetDesignationDTO
            {
                DesignationID = designation.DesignationID,
                DesignationName = designation.DesignationText
            };
        }

        public static GetCourseDTO ToGetCourseDTO(this Course course)
        {
            return new GetCourseDTO
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName
            };
        }

        public static GetCityDTO ToGetCityDTO(this City city)
        {
            return new GetCityDTO
            {
                CityID = city.CityID,
                City = city.CityName
            };
        }

        public static GetApprovalTypeDTO ToGetApprovalTypeDTO(this ApprovalType approvalType)
        {
            return new GetApprovalTypeDTO
            {
                ApprovalTypeID = approvalType.ApprovalTypeID,
                ApprovalTypeName = approvalType.Approvaltype,
            };
        }

        public static GetBloodGroupDTO ToGetBloodGroupDTO(this EmployeeBloodGroup bloodGroup)
        {
            return new GetBloodGroupDTO
            {
                BloodGroupID = bloodGroup.BloodGroupID,
                BloodGroupName = bloodGroup.BloodGroupName,
            };
        }

        public static GetCasteCategoryDTO ToGetCasteCategoryDTO(this EmployeeCasteCategory casteCategory)
        {
            return new GetCasteCategoryDTO
            {
                CasteCategoryID = casteCategory.CasteCategoryID,
                CasteCategoryName = casteCategory.CasteCategoryName,
            };
        }

        public static GetPhotoDTO ToGetPhotoPersonalDTO(this Photo photo)
        {
            return new GetPhotoDTO
            {
                PhotoID = photo.PhotoID,
                PhotoName = photo.PhotoImageName,
            };
        }

   
    }
}
