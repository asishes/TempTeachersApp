using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.ModelDTO.NonTeacherDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class NonTeacherMapper
    {
        public static Employee ToAddNonTeacherEmployee(this CreateNonTeacherDTO employeeDto, List<GetCourseDTO> availableCourses)
        {
            var employee = new Employee

            {

                EmployeeTypeID = employeeDto.DepartmentID,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                PresentAddress = employeeDto.PresentAddress,
                PermanentAddress = employeeDto.PermanentAddress,
                DateOfBirth = employeeDto.DateofBirth,
                WorkStartDate = employeeDto.DateofJoinDepartment,
                HireDate = employeeDto.DateofJoin,
                RetirementDate = employeeDto.RetirementDate,
                DesignationID = employeeDto.DesignationID,
                CategoryID = employeeDto.CategoryID,
                SchoolID = employeeDto.SchoolID,
                PhotoID = employeeDto.PhotoID,
                SchoolPositionID = employeeDto.SchoolPositionID,
                ApprovalTypeID = employeeDto.ApprovalTypeID,
                ApprovalTypeReason = employeeDto.ApprovalTypeReason,
                PromotionEligible = employeeDto.PromotionEligible ?? false, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PersonalDetails = new EmployeePersonalDetails
                {
                    SexID = employeeDto.SexID,
                    ReligionID = employeeDto.ReligionID,
                    Caste = employeeDto.Caste,
                    CasteID = employeeDto.CasteID,
                    BloodGroupID = employeeDto.BloodGroupID,
                    DistrictID = employeeDto.DistrictID,
                    DifferentlyAbled = employeeDto.DifferentlyAbled,
                    ExServiceMen = employeeDto.ExServiceMen,
                    IdentificationMark1 = employeeDto.IdentificationMark1,
                    IdentificationMark2 = employeeDto.IdentificationMark2,
                    Height = employeeDto.Height,
                    FatherName = employeeDto.FatherName,
                    MotherName = employeeDto.MotherName,
                    InterReligion = employeeDto.InterReligion,
                    MaritalStatusID = employeeDto.MaritalStatusID,
                    SpouseName = employeeDto.SpouseName,
                    SpouseReligionID = employeeDto.SpouseReligionID,
                    SpouseCaste = employeeDto.SpouseCaste,
                    PanID = employeeDto.PanID,
                    VoterID = employeeDto.VoterID,
                    AadhaarID = employeeDto.AadhaarID,
                    PFNummber = employeeDto.PFNummber,
                    PRAN = employeeDto.PRAN,
                    PEN = employeeDto.PEN,
                    ProtectedTeacher = employeeDto.ProtectedTeacher,

                },
                EmployeeEducations = employeeDto.Educations.Select(edu =>
                {
                    var course = availableCourses.FirstOrDefault(c => c.CourseID == edu.CourseID);

                    return new EmployeeEducation
                    {
                        CourseID = edu.CourseID,
                        CourseName = course != null && course.CourseName == "Others" ? edu.CourseName : null,
                        University = edu.SchoolName,
                        FromDate = edu.FromDate,
                        ToDate = edu.ToDate,
                        DocumentID = edu.DocumentID
                    };
                }).ToList(),
                 EmployeeDocuments = employeeDto.EmployeeDocuments?
            .Select(doc => new EmployeeDocument
            {
                DocumentID = doc.DocumentID
            }).ToList() ?? new List<EmployeeDocument>()
            };


            return employee;
        }

       

            public static GetNonTeacherDTO ToGetNonTeacherEmployee(this Employee employee)
            {
                var subjectData = employee.Subject != null
                   ? new Subject
                   {
                       SubjectID = employee.Subject.SubjectID,
                       SubjectText = employee.Subject.SubjectText,
                   }
                   : new Subject { SubjectID = 0, SubjectText = "No Subject assigned" };

                var departmentData = employee.EmployeeType != null
                    ? new EmployeeType
                    {
                        EmployeeTypeID = employee.EmployeeType.EmployeeTypeID,
                        Employeetype = employee.EmployeeType.Employeetype,
                    }
                    : new EmployeeType { EmployeeTypeID = 0, Employeetype = "No School assigned" };

                var categoryData = employee.EmployeeCategory != null
                    ? new EmployeeCategory
                    {
                        CategoryID = employee.EmployeeCategory.CategoryID,
                        CategoryName = employee.EmployeeCategory.CategoryName,
                    }
                    : new EmployeeCategory { CategoryID = 0, CategoryName = "No School assigned" };

                var schoolData = employee.School != null
                    ? new School
                    {
                        SchoolID = employee.School.SchoolID,
                        SchoolName = employee.School.SchoolName,
                    }
                    : new School { SchoolID = 0, SchoolName = "No School assigned" };

                var designationData = employee.Designation != null
                    ? new Designation
                    {
                        DesignationID = employee.Designation.DesignationID,
                        DesignationText = employee.Designation.DesignationText
                    }
                    : new Designation { DesignationID = 0, DesignationText = "No Designation assigned" };

                var statusData = employee.Status != null
                    ? new Status
                    {
                        StatusID = employee.Status.StatusID,
                        StatusText = employee.Status.StatusText,
                        StatusType = employee.Status.StatusType,

                    }
                    : new Status { StatusID = 0, StatusText = "No Status assigned" };

                var approvalTypeData = employee.ApprovalType != null
                    ? new ApprovalType
                    {
                        ApprovalTypeID = employee.ApprovalType.ApprovalTypeID,
                        Approvaltype = employee.ApprovalType.Approvaltype
                    }
                    : new ApprovalType { ApprovalTypeID = 0, Approvaltype = "No Approval Type assigned" };

                var supervisorData = employee.Supervisor != null
                    ? new Employee
                    {
                        EmployeeID = employee.Supervisor.EmployeeID,
                        FirstName = employee.Supervisor.FirstName + "," + employee.Supervisor.LastName,
                    }
                    : new Employee { EmployeeID = 0, FirstName = "No Supervisor Assigned" };

                var photoData = employee.Photo != null
                    ? new Photo
                    {
                        PhotoID = employee.Photo.PhotoID,
                        PhotoImageName = employee.Photo.PhotoImageName,
                    }
                    : new Photo { PhotoID = 0, PhotoImageName = "No Photo assigned" };

                var genderData = employee.PersonalDetails?.Sex != null
                    ? new EmployeeSex
                    {
                        SexID = employee.PersonalDetails.Sex.SexID,
                        Sex = employee.PersonalDetails.Sex.Sex
                    }
                    : new EmployeeSex { SexID = 0, Sex = "No Gender assigned" };

                var religionData = employee.PersonalDetails?.EmployeeReligion != null
                    ? new EmployeeReligion
                    {
                        ReligionID = employee.PersonalDetails.EmployeeReligion.ReligionID,
                        ReligionName = employee.PersonalDetails.EmployeeReligion.ReligionName,
                    }
                    : new EmployeeReligion { ReligionID = 0, ReligionName = "No Religion assigned" };

                var spouseReligionData = employee.PersonalDetails?.EmployeeSpouseReligion != null
                    ? new EmployeeReligion
                    {
                        ReligionID = employee.PersonalDetails.EmployeeSpouseReligion.ReligionID,
                        ReligionName = employee.PersonalDetails.EmployeeSpouseReligion.ReligionName,
                    }
                    : new EmployeeReligion { ReligionID = 0, ReligionName = "No Religion assigned" };



                var casteCategoryData = employee.PersonalDetails?.CasteCategory != null
                    ? new EmployeeCasteCategory
                    {
                        CasteCategoryID = employee.PersonalDetails.CasteCategory.CasteCategoryID,
                        CasteCategoryName = employee.PersonalDetails.CasteCategory.CasteCategoryName
                    }
                    : new EmployeeCasteCategory { CasteCategoryID = 0, CasteCategoryName = "No Caste Category assigned" };

                var bloodGroupData = employee.PersonalDetails?.BloodGroup != null
                    ? new EmployeeBloodGroup
                    {
                        BloodGroupID = employee.PersonalDetails.BloodGroup.BloodGroupID,
                        BloodGroupName = employee.PersonalDetails.BloodGroup.BloodGroupName,
                    }
                    : new EmployeeBloodGroup { BloodGroupID = 0, BloodGroupName = "No Blood Group assigned" };

                var districtData = employee.PersonalDetails?.District != null
                    ? new District
                    {
                        DistrictID = employee.PersonalDetails.District.DistrictID,
                        DistrictName = employee.PersonalDetails.District.DistrictName,
                    }
                    : new District { DistrictID = 0, DistrictName = "No District assigned" };

                var maritalStatusData = employee.PersonalDetails?.MaritalStatus != null
                    ? new EmployeeMaritalStatus
                    {
                        MaritalStatusID = employee.PersonalDetails.MaritalStatus.MaritalStatusID,
                        MaritalStatusName = employee.PersonalDetails.MaritalStatus.MaritalStatusName,
                    }
                    : new EmployeeMaritalStatus { MaritalStatusID = 0, MaritalStatusName = "No Marital Status assigned" };

                var employeeEducationData = employee.EmployeeEducations.Any()
        ? employee.EmployeeEducations.Select(education => new GetEmployeeEducationDTO
        {
            EducationTypeID = education.Course?.EducationTypeID ?? 0, // or a default value
            EducationTypeName = education.Course?.EducationType.EductionTypeName ?? null,
            CourseID = education.CourseID,
            CourseName = education.CourseName,
            CourseText = education.Course?.CourseName ?? "No Course Name", // fallback value
            SchoolName = education.University,
            FromDate = education.FromDate,
            ToDate = education.ToDate,
            DocumentID = education.DocumentID,
            Documentpath = education.Document?.DocumentFileName ?? "No Document" // fallback value
        }).ToList()
        : new List<GetEmployeeEducationDTO> { new GetEmployeeEducationDTO { CourseName = "No Education assigned" } };

                var employeeDocumentData = employee.EmployeeDocuments?.Select(document => new GetEmployeeDocumentDTO
                {
                    DocumentID = document.DocumentID,
                    Documentpath = document.Document?.DocumentFileName ?? "No Document",
                }).ToList() ?? new List<GetEmployeeDocumentDTO>();


                var employeeDTO = new GetNonTeacherDTO
                {
                    EmployeeID = employee.EmployeeID,
                    UniqueID = employee.UniqueID,
                    FirstName = employee.FirstName ?? "N/A", // default if null
                    LastName = employee.LastName ?? "N/A", // default if null
                    Email = employee.Email ?? "N/A",
                    Phone = employee.Phone ?? "N/A",
                    PresentAddress = employee.PresentAddress ?? "N/A",
                    PermanentAddress = employee.PermanentAddress ?? "N/A",
                    DateOfBirth = employee.DateOfBirth ?? DateTime.MinValue, // default date if null
                    DateofDepartmentJoin = employee.WorkStartDate ?? DateTime.MinValue,
                    DateofJoin = employee.HireDate ?? DateTime.MinValue,
                    RetirementDate = employee.RetirementDate ?? DateTime.MinValue,
                    SchoolPositionID = employee.SchoolPositionID ?? 0, // default integer if null
                    PromotionEligible = employee.PromotionEligible,
                    ApprovalTypeReason = employee.ApprovalTypeReason,
                    CasteName = employee.PersonalDetails?.Caste ?? "Not Specified",
                    DifferentlyAbled = employee.PersonalDetails?.DifferentlyAbled ?? false,
                    ExServiceMen = employee.PersonalDetails?.ExServiceMen ?? false,
                    IdentificationMark1 = employee.PersonalDetails?.IdentificationMark1 ?? "N/A",
                    IdentificationMark2 = employee.PersonalDetails?.IdentificationMark2 ?? "N/A",
                    Height = employee.PersonalDetails?.Height ?? 0.0, // default if null
                    FatherName = employee.PersonalDetails?.FatherName ?? "N/A",
                    MotherName = employee.PersonalDetails?.MotherName ?? "N/A",
                    InterReligion = employee.PersonalDetails?.InterReligion ?? false,
                    SpouseName = employee.PersonalDetails?.SpouseName ?? "N/A",
                    SpouseCaste = employee.PersonalDetails?.SpouseCaste ?? "Not Specified",
                    PanID = employee.PersonalDetails?.PanID ?? "N/A",
                    VoterID = employee.PersonalDetails?.VoterID ?? "N/A",
                    AadhaarID = employee.PersonalDetails?.AadhaarID ?? "N/A",
                    PFNumber = employee.PersonalDetails?.PFNummber ?? "N/A",
                    PRAN = employee.PersonalDetails?.PRAN ?? "N/A",
                    PEN = employee.PersonalDetails?.PEN ?? "N/A",
                    EligibilityTestQualified = employee.PersonalDetails?.EligibilityTestQualified ?? false,
                    ProtectedTeacher = employee.PersonalDetails?.ProtectedTeacher ?? false,
                    MaritalStatusDTO = maritalStatusData.ToGetEmployeeMaritalStatusDTO(),
                    SpouseReligionDTO = spouseReligionData.ToGetEmployeeReligionDTO(),
                    bloodGroupDTO = bloodGroupData.ToGetBloodGroupDTO(),
                    districtDTO = districtData.ToGetDistrictDTO(),
                    genderDTO = genderData.ToGetEmployeeGenderDTO(),
                    religionDTO = religionData.ToGetEmployeeReligionDTO(),
                    casteCategoryDTO = casteCategoryData.ToGetCasteCategoryDTO(),
                    Department = departmentData.ToGetEmployeeTypeDTO(),
                    designationDTO = designationData.ToGetDesignationDTO(),
                    GetEmployeeCategoryDTO = categoryData.ToGetEmployeeCategoryDTO(),
                    schoolDTO = schoolData.GetSchoolDTO(),
                    photoDTO = photoData.ToGetPhotoPersonalDTO(),
                    getSubjectDTO = subjectData.ToGetSubjectDTO(),
                    statusDTO = statusData.ToGetStatusDTO(),
                    GetApprovalTypeDTO = approvalTypeData.ToGetApprovalTypeDTO(),
                    Supervisor = supervisorData.ToGetEmployeePersonalDTO(),
                    GetEducations = employeeEducationData
                };

                return employeeDTO;
            }


            public static NonTeacherList GetNonTeacherEmployeeListDTO(this Employee employee)
        {
            var currentDate = DateTime.Now;
            return new NonTeacherList
            {
                TeacherId = employee.EmployeeID,
                Name = $"{employee.FirstName ?? "Unknown"} {employee.LastName ?? "Unknown"}", // Handle null values
                SchoolId = employee.SchoolID,
                SchoolName = employee.School?.SchoolName ?? "No School Assigned", // Handle null school
                Designation = employee.Designation?.DesignationText ?? "No Designation", // Handle null designation
                ExperienceYear = employee.HireDate != null ? CalculateExperience(employee.HireDate, currentDate) : 0, // Calculate experience if work start date exists
                Age = employee.DateOfBirth != null ? CalculateExactAge(employee.DateOfBirth, currentDate) : 0, // Calculate age if date of birth exists
                PhoneNumber = employee.Phone ?? "No Phone Number",
                Status = employee.Status.StatusText ??string.Empty,
                 TeacherPopUpDTO = new TeacherPopUpDTO
                 {
                     Photo = employee.Photo != null ? employee.Photo.PhotoImageName : null,
                     Name = $"{employee.FirstName ?? "Unknown"} {employee.LastName ?? "Unknown"}",
                     Subject = employee.Subject?.SubjectText ?? "No Subject Assigned",
                     SchoolName = employee.School != null
            ? $"{employee.School.SchoolName ?? "No School Name"}, " +
              $"{employee.School.Address ?? "No Address"}, " +
              $"{employee.School.City?.CityName ?? "No City Assigned"}, " +
              $"{employee.School.State ?? "No State Assigned"}, " +
              $"{employee.School.Pincode ?? "No Pincode"}"
            : "No School Assigned",
                     Email = employee.Email ?? "N/A",
                     PhoneNumber = employee.Phone ?? "N/A",
                     DateofJoin = employee.HireDate,
                     ReportedTo = employee.Supervisor != null
            ? $"{employee.Supervisor.FirstName ?? "Unknown"} {employee.Supervisor.LastName ?? "Unknown"}"
            : "N/A",
                 }
            };
        }
        private static int CalculateExperience(DateTime? hireDate, DateTime currentDate)
        {
            if (hireDate == null)
                return 0;

            var startDate = hireDate.Value;
            int experience = currentDate.Year - startDate.Year;

            // If the current date is before the anniversary of the hire date in the current year, subtract 1
            if (currentDate < startDate.AddYears(experience))
                experience--;

            return experience;
        }

        private static int CalculateExactAge(DateTime? dateOfBirth, DateTime currentDate)
        {
            if (dateOfBirth == null)
            {
                // Return 0 or throw an exception if age cannot be calculated without a date of birth
                return 0;
            }

            DateTime dob = dateOfBirth.Value;
            int age = currentDate.Year - dob.Year;
            int monthDifference = currentDate.Month - dob.Month;
            int dayDifference = currentDate.Day - dob.Day;

            // Adjust for month and day
            if (monthDifference < 0 || (monthDifference == 0 && dayDifference < 0))
            {
                age--;
            }

            return age;
        }
        public static Employee ToUpdateNonTeacherEmployee(this UpdateNonTeacherDTO teacherUpdate)
        {
            return new Employee
            {
                EmployeeTypeID = teacherUpdate.DepartmentID,
                FirstName = teacherUpdate.FirstName,
                LastName = teacherUpdate.LastName,
                Email = teacherUpdate.Email,
                Phone = teacherUpdate.Phone,
                PresentAddress = teacherUpdate.PresentAddress,
                PermanentAddress = teacherUpdate.PermanentAddress,
                PhotoID = teacherUpdate.PhotoID,
                DateOfBirth = teacherUpdate.DateofBirth,
                WorkStartDate = teacherUpdate.DateofJoinDepartment,
                HireDate = teacherUpdate.DateofJoin, 
                RetirementDate = teacherUpdate.RetirementDate,
                DesignationID = teacherUpdate.DesignationID,
                CategoryID = teacherUpdate.CategoryID,
                SchoolID = teacherUpdate.SchoolID,
                SchoolPositionID = teacherUpdate.SchoolPositionID,
                ApprovalTypeID = teacherUpdate.ApprovalTypeID,
                ApprovalTypeReason = teacherUpdate.ApprovalTypeReason,
                UpdatedAt = DateTime.Now,

                // Map EmployeeEducations if any
                EmployeeEducations = teacherUpdate.Educations?.Select(e => new EmployeeEducation
                {
                    CourseID = e.CourseID,
                    CourseName = e.CourseName,
                    University = e.SchoolName,
                    FromDate = e.FromDate,
                    ToDate = e.ToDate,
                    DocumentID = e.DocumentID
                }).ToList(),
                EmployeeDocuments = teacherUpdate.EmployeeDocuments?.Select(e => new EmployeeDocument
                {
                    DocumentID = e.DocumentID
                }).ToList(),

                PersonalDetails = new EmployeePersonalDetails
                {
                    PEN = teacherUpdate.PEN,
                    SexID = teacherUpdate.SexID,
                    ReligionID = teacherUpdate.ReligionID,
                    CasteID = teacherUpdate.CasteID,
                    Caste = teacherUpdate.Caste,
                    BloodGroupID = teacherUpdate.BloodGroupID,
                    DifferentlyAbled = teacherUpdate.DifferentlyAbled,
                    ExServiceMen = teacherUpdate.ExServiceMen,
                    IdentificationMark1 = teacherUpdate.IdentificationMark1,
                    IdentificationMark2 = teacherUpdate.IdentificationMark2,
                    Height = teacherUpdate.Height,
                    FatherName = teacherUpdate.FatherName,
                    MotherName = teacherUpdate.MotherName,
                    InterReligion = teacherUpdate.InterReligion,
                    MaritalStatusID = teacherUpdate.MaritalStatusID,
                    SpouseName = teacherUpdate.SpouseName,
                    SpouseReligionID = teacherUpdate.SpouseReligionID,
                    SpouseCaste = teacherUpdate.SpouseCaste,
                    PanID = teacherUpdate.PanID,
                    VoterID = teacherUpdate.VoterID,
                    AadhaarID = teacherUpdate.AadhaarID,
                    PFNummber = teacherUpdate.PFNummber,
                    PRAN = teacherUpdate.PRAN,
                    DistrictID = teacherUpdate.DistrictID,
                    ProtectedTeacher = teacherUpdate.ProtectedTeacher,
                    EligibilityTestQualified = teacherUpdate.EligibilityTestQualified,
                    
                }

            };

        }
        public static TeacherListDTO GetCommonNonTeacherListDTO(this Employee employee)
        {
            var currentDate = DateTime.Now;
            return new TeacherListDTO
            {
                TeacherId = employee.EmployeeID,
                Name = $"{employee.FirstName ?? "Unknown"} {employee.LastName ?? "Unknown"}", // Handle null values
                SchoolId = employee.SchoolID,
                SchoolName = employee.School?.SchoolName ?? "No School Assigned", // Handle null school
                Designation = employee.Designation?.DesignationText ?? "No Designation", // Handle null designation
                Subject = employee.Subject?.SubjectText != null ? string.Join(", ", employee.Subject.SubjectText) : "No Subject Assigned", // Handle null subject
                ExperienceYear = employee.HireDate != null ? CalculateExperience(employee.WorkStartDate, currentDate) : 0, // Calculate experience if work start date exists
                Age = employee.DateOfBirth != null ? CalculateExactAge(employee.DateOfBirth, currentDate) : 0, // Calculate age if date of birth exists
                PhoneNumber = employee.Phone ?? "No Phone Number",
                Status = employee.Status.StatusText ?? string.Empty,
               
                TeacherPopUpDTO = new TeacherPopUpDTO
                {
                    Photo = employee.Photo != null ? employee.Photo.PhotoImageName : null,
                    Name = $"{employee.FirstName ?? "Unknown"} {employee.LastName ?? "Unknown"}",
                    Subject = employee.Subject?.SubjectText ?? "No Subject Assigned",
                    SchoolName = employee.School != null
            ? $"{employee.School.SchoolName ?? "No School Name"}, " +
              $"{employee.School.Address ?? "No Address"}, " +
              $"{employee.School.City?.CityName ?? "No City Assigned"}, " +
              $"{employee.School.State ?? "No State Assigned"}, " +
              $"{employee.School.Pincode ?? "No Pincode"}"
            : "No School Assigned",
                    Email = employee.Email ?? "N/A",
                    PhoneNumber = employee.Phone ?? "N/A",
                    DateofJoin = employee.HireDate,
                    ReportedTo = employee.Supervisor != null
            ? $"{employee.Supervisor.FirstName ?? "Unknown"} {employee.Supervisor.LastName ?? "Unknown"}"
            : "N/A",
                }
            };
        }
       
    }
}
