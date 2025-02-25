using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.ModelDTO.PhotoDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolClassDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;

namespace TeachersApp.Services.Mapper
{
    public static class SchoolMapper
    {
        public static School ToAddSchool(this AddSchoolDTO schoolDto)
        {
            var school = new School
            ///return new school
            {
                SchoolName = schoolDto.SchoolName,
                SchoolStandardTypes = schoolDto.AddSchoolTypes?.Select(d => new SchoolStandardType
                {
                    SchoolTypeID = d.SchoolTypeID,

                }).ToList() ?? new List<SchoolStandardType>(),
                Address = schoolDto.Address,
                CityID = schoolDto.CityID,
                State = schoolDto.State,
                Pincode = schoolDto.Pincode,
                Email = schoolDto.Email,
                Phone = schoolDto.Phone,
                PhotoID = schoolDto.PhotoID,
                PrincipalID = schoolDto.PrincipalID,
                VicePrincipalID = schoolDto.VicePrincipalID,
                // Map AddDivisions to the Division entity list
                SchoolClasses = schoolDto.AddClasses?.Select(d => new SchoolClass
                {
                    Class = d.Class,
                    SchoolDivisionCounts = d.AddDivisions?.Select(div => new SchoolDivisionCount
                    {
                        Division = div.Division,
                        StudentCount = div.StudentCount
                    }).ToList() ?? new List<SchoolDivisionCount>()
                }).ToList() ?? new List<SchoolClass>()
            };

            return school;
        }

        public static GetSchoolDTO ToGetSchoolDTO(this School school)
        {
            return new GetSchoolDTO
            {
                SchoolID = school.SchoolID,
                SchoolName = school.SchoolName,
                getSchoolTypes = school.SchoolStandardTypes?.Select(e => new GetSchoolTypeWithSchoolDTO
                {
                    SchoolTypeName = e.SchoolType.SchoolTypeName,
                   
                }).ToList() ?? new List<GetSchoolTypeWithSchoolDTO>(),
                Address = school.Address,
                CityID = school.CityID,
                State = school.State,
                Pincode = school.Pincode,
                Email = school.Email,
                Phone = school.Phone,
                PhotoID = school.PhotoID,
                Photo = school.Photo != null ? school.Photo.PhotoImageName : null,
                StatusID = school.StatusID,
                PrincipalID = school.PrincipalID,
                VicePrincipalID = school.VicePrincipalID,
                CityName = school.City != null ? school.City.CityName : null,    // Assuming City has a CityName property
                PrincipalName = school.Principal != null ? school.Principal.FirstName : null, // Assuming Principal is an Employee with a EmployeeName property
                PrincipalPhone = school.Principal != null ? school.Principal.Phone : null,
                VicePrincipalName = school.VicePrincipal != null ? school.VicePrincipal.FirstName : null, // Assuming VicePrincipal is an Employee with a EmployeeName property
                VicePrincipalPhone = school.VicePrincipal != null ? school.VicePrincipal.Phone : null,
                StatusName = school.Status != null ? school.Status.StatusText : null,
                // Assuming Status has a StatusName property
                getClasses = school.SchoolClasses?.Select(e => new GetClassWithSchoolDTO
                {
                    Class = e.Class,
                    getDivisions = e.SchoolDivisionCounts?.Select(div => new GetDivitionWithClassDTO
                    {
                        Division = div.Division,
                        StudentCount = div.StudentCount,
                    }).ToList() ?? new List<GetDivitionWithClassDTO>(),
                    TotalDivisions = e.SchoolDivisionCounts?.Count() ?? 0, // Get total count of divisions
                    TotalStudents = e.SchoolDivisionCounts?.Sum(div => div.StudentCount) ?? 0

                }).ToList() ?? new List<GetClassWithSchoolDTO>(),
            };
        }


        public static SchoolListDTO ToGetSchoolListDTO(this School school)
        {
            return new SchoolListDTO
            {
                SchoolId = school.SchoolID,
                Name = school.SchoolName ?? string.Empty,
                getSchoolTypes = school.SchoolStandardTypes?.Select(e => new GetSchoolTypeWithSchoolDTO
                {
                    SchoolTypeName = e.SchoolType.SchoolTypeName,

                }).ToList() ?? new List<GetSchoolTypeWithSchoolDTO>(),

                Contact = school.Phone ?? string.Empty,
                City = school.City.CityName?? "N/A",
                Principal = school.Principal?.FirstName ?? "N/A", // Handles null Principal
                NoOfTeachers = school.Employees?.Count() ?? 0,
                NoOfStudents = school.SchoolClasses
                    .SelectMany(sc => sc.SchoolDivisionCounts) // Flatten the collection
                    .Sum(sd => sd.StudentCount), // Sum up the student count across all divisions
                getClasses = school.SchoolClasses?.Select(e => new GetClassWithSchoolDTO
                {
                    Class = e.Class,
                    getDivisions = e.SchoolDivisionCounts?.Select(div => new GetDivitionWithClassDTO
                    {
                        Division = div.Division,
                        StudentCount = div.StudentCount
                    }).ToList() ?? new List<GetDivitionWithClassDTO>()
                }).ToList() ?? new List<GetClassWithSchoolDTO>(),
            };
        }

        public static SchoolPopUpDTO SchoolPopUpDTO(this School school)
        {
            return new SchoolPopUpDTO
            {
                Photo = school.Photo != null ? school.Photo.PhotoImageName : null,
                PhotoId = school.PhotoID,
                Name = school.SchoolName ?? string.Empty,
                Address = school.Address ?? string.Empty,
                City = school.City.CityName ?? string.Empty,
                Email = school.Email ?? string.Empty,
                Phone = school.Phone ?? string.Empty,
                Principal = school.Principal != null
                    ? $"{school.Principal.FirstName} {school.Principal.LastName}"
                    : "N/A",
                PrincipalPhotoPath = school.Principal?.Photo?.PhotoImageName ?? null,

                VicePrincipal = school.VicePrincipal != null
                        ? $"{school.VicePrincipal.FirstName} {school.VicePrincipal.LastName}"
                        : "N/A",
                VicePrincipalPhotoPath = school.VicePrincipal?.Photo?.PhotoImageName ?? null


            };
        }

        public static GetAllSchoolsWithCityDTO ToGetSchoolWithCity(this School school)
        {
            return new GetAllSchoolsWithCityDTO
            { 
                SchoolID = school.SchoolID, 
                SchoolName = school.SchoolName,
                CityID = school.CityID,
                CityName=school.City.CityName,
            };
        }

        public static School ToUpdateSchool(this UpdateSchoolDTO updateSchool)
        {
            return new School
            {
                SchoolName = updateSchool.SchoolName,
                SchoolStandardTypes = updateSchool.updateSchoolTypes?.Select(d => new SchoolStandardType
                {
                    SchoolTypeID = d.SchoolTypeID,
                    
                }).ToList() ?? new List<SchoolStandardType>(),
                Address = updateSchool.Address,
                CityID = updateSchool.CityID,
                State = updateSchool.State,
                Pincode = updateSchool.Pincode,
                Email = updateSchool.Email,
                Phone = updateSchool.Phone,
                PhotoID = updateSchool.PhotoID,
                PrincipalID = updateSchool.PrincipalID,
                VicePrincipalID = updateSchool.VicePrincipalID,
                // Map AddDivisions to the Division entity list
                SchoolClasses = updateSchool.updateClasses?.Select(d => new SchoolClass
                {
                    Class = d.Class,
                    SchoolDivisionCounts = d.updateDivisions?.Select(div => new SchoolDivisionCount
                    {
                        Division = div.Division,
                        StudentCount = div.StudentCount
                    }).ToList() ?? new List<SchoolDivisionCount>()
                }).ToList() ?? new List<SchoolClass>()
            };
        }

        public static SchoolWithAuthorityDTO ToGetSchoolWithAuthorityDTO(this School school)
        {
            return new SchoolWithAuthorityDTO
            {
                SchoolID = school.SchoolID,
                SchoolName = school.SchoolName,
                City = school.City?.CityName ?? null,
                Email = school.Email ?? null,
                Address = school.Address ?? null,
                PrincipalName = school.Principal?.FirstName ?? null,
                PrincipalPhone = school.Principal?.Phone ?? null,
                VicePrincipalName = school.VicePrincipal?.FirstName ?? null,
                VicePrincipalPhone = school.VicePrincipal?.Phone ?? null,
            };
        }
    }
}
