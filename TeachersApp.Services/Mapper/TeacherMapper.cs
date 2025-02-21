using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.DocumentDTO;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.ModelDTO.PhotoDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class TeacherMapper
    {
        public static Employee ToAddEmployee(this CreateEmployeeDTO employeeDto, List<GetCourseDTO> availableCourses)
        {
            var employee = new Employee
            {
                EmployeeTypeID = employeeDto.DepartmentID,
                FirstName = !string.IsNullOrWhiteSpace(employeeDto.FirstName) ? employeeDto.FirstName : "Unknown",
                LastName = !string.IsNullOrWhiteSpace(employeeDto.LastName) ? employeeDto.LastName : "Unknown",
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
                SubjectID = employeeDto.SubjectID,
                SchoolPositionID = employeeDto.SchoolPositionID,
                ApprovalTypeID = employeeDto.ApprovalTypeID,
                ApprovalTypeReason = employeeDto.ApprovalTypeReason,
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
                    EligibilityTestQualified = employeeDto.EligibilityTestQualified,
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
    




        public static TeacherListDTO GetEmployeeListDTO(this Employee employee)
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
                ExperienceYear = employee.HireDate != null ? CalculateExperience(employee.HireDate, currentDate) : 0, // Calculate experience if work start date exists
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

        public static TeacherListDTO GetEmployeeFilterListDTO(this Employee employee)
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

       
    

       

        



        public static TeacherPopUpDTO ToGetTeacherPopUpDTO(this Employee employee)
        {
            return new TeacherPopUpDTO
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

            };
        }

        public static Employee ToUpdateEmployee(this TeacherUpdateDTO teacherUpdate)
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
                RetirementDate =teacherUpdate.DateofRetirement,
                DesignationID = teacherUpdate.DesignationID,
                ApprovalTypeID = teacherUpdate.ApprovalTypeID,
                ApprovalTypeReason = teacherUpdate.ApprovalTypeReason,
                CategoryID = teacherUpdate.CategoryID,
                SchoolID = teacherUpdate.SchoolID,
                SubjectID = teacherUpdate.SubjectID,
                SchoolPositionID = teacherUpdate.SchoolPositionID,
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
                    EligibilityTestQualified = teacherUpdate.EligibilityTestQualified ?? false,
                }

            };
        }
        public static GetEmployeeLiteDTO ToGetEmployee(this Employee employee)
        {
            var subjectData = employee.Subject != null
               ? new Subject
               {
                   SubjectID = employee.Subject.SubjectID,
                   SubjectText = employee.Subject.SubjectText,
               }
               : new Subject { SubjectID = 0, SubjectText = string.Empty };

            var departmentData = employee.EmployeeType != null
                ? new EmployeeType
                {
                    EmployeeTypeID = employee.EmployeeType.EmployeeTypeID,
                    Employeetype = employee.EmployeeType.Employeetype,
                }
                : new EmployeeType { EmployeeTypeID = 0, Employeetype = string.Empty };

            var categoryData = employee.EmployeeCategory != null
                ? new EmployeeCategory
                {
                    CategoryID = employee.EmployeeCategory.CategoryID,
                    CategoryName = employee.EmployeeCategory.CategoryName,
                }
                : new EmployeeCategory { CategoryID = 0, CategoryName = string.Empty };

            var schoolData = employee.School != null
                 ? new School
                 {
                     SchoolID = employee.School.SchoolID,
                     SchoolName = employee.School.SchoolName ?? string.Empty,
                     City = employee.School.City != null
                         ? new City { CityName = employee.School.City.CityName ?? string.Empty}
                         : new City { CityName = string.Empty }
                 }
                 : new School
                 {
                     SchoolID = 0,
                     SchoolName = string.Empty,
                     City = new City { CityName = string.Empty }
                 };


            var designationData = employee.Designation != null
                ? new Designation
                {
                    DesignationID = employee.Designation.DesignationID,
                    DesignationText = employee.Designation.DesignationText
                }
                : new Designation { DesignationID = 0, DesignationText = string.Empty };

            var statusData = employee.Status != null
                ? new Status
                {
                    StatusID = employee.Status.StatusID,
                    StatusText = employee.Status.StatusText,
                    StatusType = employee.Status.StatusType,    
                   
                }
                : new Status { StatusID = 0, StatusText = string.Empty };

            var approvalTypeData = employee.ApprovalType != null
                ? new ApprovalType
                {
                    ApprovalTypeID = employee.ApprovalType.ApprovalTypeID,
                    Approvaltype = employee.ApprovalType.Approvaltype
                }
                : new ApprovalType { ApprovalTypeID = 0, Approvaltype = string.Empty };

            var supervisorData = employee.Supervisor != null
                ? new Employee
                {
                    EmployeeID = employee.Supervisor.EmployeeID,
                    FirstName = employee.Supervisor.FirstName + "," + employee.Supervisor.LastName,
                }
                : new Employee { EmployeeID = 0, FirstName = string.Empty };

            var photoData = employee.Photo != null
                ? new Photo
                {
                    PhotoID = employee.Photo.PhotoID,
                    PhotoImageName = employee.Photo.PhotoImageName,
                }
                : new Photo { PhotoID = 0, PhotoImageName = string.Empty };

            var genderData = employee.PersonalDetails?.Sex != null
                ? new EmployeeSex
                {
                    SexID = employee.PersonalDetails.Sex.SexID,
                    Sex = employee.PersonalDetails.Sex.Sex
                }
                : new EmployeeSex { SexID = 0, Sex = string.Empty };

            var religionData = employee.PersonalDetails?.EmployeeReligion != null
                ? new EmployeeReligion
                {
                    ReligionID = employee.PersonalDetails.EmployeeReligion.ReligionID,
                    ReligionName = employee.PersonalDetails.EmployeeReligion.ReligionName,
                }
                : new EmployeeReligion { ReligionID = 0, ReligionName = string.Empty };

            var spouseReligionData = employee.PersonalDetails?.EmployeeSpouseReligion != null
                ? new EmployeeReligion
                {
                    ReligionID = employee.PersonalDetails.EmployeeSpouseReligion.ReligionID,
                    ReligionName = employee.PersonalDetails.EmployeeSpouseReligion.ReligionName,
                }
                : new EmployeeReligion { ReligionID = 0, ReligionName = string.Empty };



            var casteCategoryData = employee.PersonalDetails?.CasteCategory != null
                ? new EmployeeCasteCategory
                {
                    CasteCategoryID = employee.PersonalDetails.CasteCategory.CasteCategoryID,
                    CasteCategoryName = employee.PersonalDetails.CasteCategory.CasteCategoryName
                }
                : new EmployeeCasteCategory { CasteCategoryID = 0, CasteCategoryName = string.Empty };

            var bloodGroupData = employee.PersonalDetails?.BloodGroup != null
                ? new EmployeeBloodGroup
                {
                    BloodGroupID = employee.PersonalDetails.BloodGroup.BloodGroupID,
                    BloodGroupName = employee.PersonalDetails.BloodGroup.BloodGroupName,
                }
                : new EmployeeBloodGroup { BloodGroupID = 0, BloodGroupName = string.Empty };

            var districtData = employee.PersonalDetails?.District != null
                ? new District
                {
                    DistrictID = employee.PersonalDetails.District.DistrictID,
                    DistrictName = employee.PersonalDetails.District.DistrictName,
                }
                : new District { DistrictID = 0, DistrictName = string.Empty };

            var maritalStatusData = employee.PersonalDetails?.MaritalStatus != null
                ? new EmployeeMaritalStatus
                {
                    MaritalStatusID = employee.PersonalDetails.MaritalStatus.MaritalStatusID,
                    MaritalStatusName = employee.PersonalDetails.MaritalStatus.MaritalStatusName,
                }
                : new EmployeeMaritalStatus { MaritalStatusID = 0, MaritalStatusName = string.Empty };

            var employeeEducationData = employee.EmployeeEducations.Any()
    ? employee.EmployeeEducations.Select(education => new GetEmployeeEducationDTO
    {
        EducationTypeID = education.Course?.EducationTypeID ?? 0, // or a default value
        EducationTypeName = education.Course?.EducationType.EductionTypeName ?? string.Empty,
        CourseID = education.CourseID,
        CourseName = education.CourseName ?? string.Empty,
        CourseText = education.Course?.CourseName ?? string.Empty, // fallback value
        SchoolName = education.University ?? string.Empty,
        FromDate = education.FromDate,
        ToDate = education.ToDate,
        DocumentID = education.DocumentID,
        Documentpath = education.Document?.DocumentFileName ?? string.Empty // fallback value
    }).ToList()
    : new List<GetEmployeeEducationDTO> { new GetEmployeeEducationDTO { CourseName = string.Empty } };

            var employeeDocumentData = employee.EmployeeDocuments != null && employee.EmployeeDocuments.Any()
       ? employee.EmployeeDocuments.Select(document => new GetEmployeeDocumentDTO
       {
           DocumentID = document.DocumentID,
           DocumentName = document.Document?.DocumentText ??  string.Empty,
           Documentpath = document.Document?.DocumentFileName ?? string.Empty,
       }).ToList()
       : new List<GetEmployeeDocumentDTO>();


            var employeeDTO = new GetEmployeeLiteDTO
            {
                EmployeeID = employee.EmployeeID,
                UniqueID = employee.UniqueID,
                FirstName = employee.FirstName ?? string.Empty, 
                LastName = employee.LastName ?? string.Empty,
                Email = employee.Email ?? string.Empty,
                Phone = employee.Phone ?? string.Empty,
                PresentAddress = employee.PresentAddress ?? string.Empty,
                PermanentAddress = employee.PermanentAddress ?? string.Empty,
                DateOfBirth = employee.DateOfBirth ?? (DateTime?)null, 
                DateofDepartmentJoin = employee.WorkStartDate ?? (DateTime?)null,
                DateofJoin = employee.HireDate ?? (DateTime?)null,
                RetirementDate = employee.RetirementDate ?? (DateTime?)null,
                SchoolPositionID = employee.SchoolPositionID ?? (int?)null, 
                PromotionEligible = employee.PromotionEligible,
                ApprovalTypeReason = employee.ApprovalTypeReason,
                CasteName = employee.PersonalDetails?.Caste ?? string.Empty,
                DifferentlyAbled = employee.PersonalDetails?.DifferentlyAbled ?? (bool?)null,
                ExServiceMen = employee.PersonalDetails?.ExServiceMen ?? (bool?)null,
                IdentificationMark1 = employee.PersonalDetails?.IdentificationMark1 ?? string.Empty,
                IdentificationMark2 = employee.PersonalDetails?.IdentificationMark2 ?? string.Empty,
                Height = employee.PersonalDetails?.Height ?? (double?)null,
                FatherName = employee.PersonalDetails?.FatherName ??  string.Empty,
                MotherName = employee.PersonalDetails?.MotherName ?? string.Empty,
                InterReligion = employee.PersonalDetails?.InterReligion ?? (bool?)null,
                SpouseName = employee.PersonalDetails?.SpouseName ?? string.Empty,
                SpouseCaste = employee.PersonalDetails?.SpouseCaste ?? string.Empty,
                PanID = employee.PersonalDetails?.PanID ?? string.Empty,
                VoterID = employee.PersonalDetails?.VoterID ?? string.Empty,
                AadhaarID = employee.PersonalDetails?.AadhaarID ?? string.Empty,
                PFNumber = employee.PersonalDetails?.PFNummber ?? string.Empty,
                PRAN = employee.PersonalDetails?.PRAN ?? string.Empty,
                PEN = employee.PersonalDetails?.PEN ?? string.Empty,
                EligibilityTestQualified = employee.PersonalDetails?.EligibilityTestQualified ?? (bool?)null,
                ProtectedTeacher = employee.PersonalDetails?.ProtectedTeacher ?? (bool?)null,
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
                GetEducations = employeeEducationData,
                GetEmployeeDocuments = employeeDocumentData
            };

            return employeeDTO;
        }
        
        public static StaffListDTO ToGetStaffListDTO ( this Employee employee )
        {
            return new StaffListDTO
            {
                Name = $"{employee.FirstName ?? string.Empty} {employee.LastName ?? string.Empty}",
                DOB = employee.DateOfBirth,
                Phone = employee.Phone ?? string.Empty,
                JoinDate = employee.HireDate,
                Position = employee.Designation?.DesignationText ?? string.Empty,
                Subject = employee.Subject?.SubjectText ?? string.Empty,
                EmployeeType = employee.EmployeeType?.Employeetype ?? string.Empty,

                TeacherPopUpDTO = new TeacherPopUpDTO
                {
                    Photo = employee.Photo?.PhotoImageName ?? null,
                    Name = $"{employee.FirstName ?? string.Empty} {employee.LastName ?? string.Empty}",
                    Subject = employee.Subject?.SubjectText ?? string.Empty,
                    SchoolName = employee.School != null
                ? $"{employee.School.SchoolName ?? string.Empty}, " +
                  $"{employee.School.Address ?? string.Empty}, " +
                  $"{employee.School.City?.CityName ?? string.Empty}, " +
                  $"{employee.School.State ?? string.Empty}, " +
                  $"{employee.School.Pincode ?? string.Empty}"
                : string.Empty,
                    Email = employee.Email ?? null,
                    PhoneNumber = employee.Phone ?? string.Empty,
                    DateofJoin = employee.HireDate,
                    ReportedTo = employee.Supervisor != null
                ? $"{employee.Supervisor.FirstName ?? string.Empty} {employee.Supervisor.LastName ?? string.Empty}"
                : string.Empty,
                }
            };
        }

        public static GetDesignationDTO ToGetPromotionDesignationDTO(this Employee employee)
        {
            return new GetDesignationDTO
            {
                DesignationID = employee.DesignationID ?? 0,
                DesignationName = employee.Designation.DesignationText,
            };
        }

        public static GetEmployeeDTO ToGetPromotionEligibleEmployeeDTO (this Employee employee)
        {
            return new GetEmployeeDTO
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = $"{employee.FirstName ?? "Unknown"} {employee.LastName ?? "Unknown"}",
            };
        }

        public static EmployeeOnLeaveDTO GetEmployeeLeaveDTO(this Employee employee)
        {
            var currentDate = DateTime.Now;

            var completedDays = employee.LeaveRequests
                .Where(lr => lr.ToDate < currentDate) 
                .Sum(lr => (lr.ToDate - lr.FromDate).Days + 1); 

            var remainingDays = employee.LeaveRequests
                .Where(lr => lr.ToDate >= currentDate)
                .Sum(lr => (int)Math.Ceiling((lr.ToDate - currentDate).TotalDays));

            return new EmployeeOnLeaveDTO
            {
                TeacherId = employee.EmployeeID,
                Name = $"{employee.FirstName ?? string.Empty} {employee.LastName ?? string.Empty}", 
                SchoolId = employee.SchoolID,
                SchoolName = employee.School?.SchoolName ?? string.Empty, 
                Designation = employee.Designation?.DesignationText ?? string.Empty,
                Subject = employee.Subject?.SubjectText ?? string.Empty,
                LeaveStartDate = employee.LeaveRequests.OrderByDescending(lr => lr.FromDate).FirstOrDefault()?.FromDate,
                LeaveEndDate = employee.LeaveRequests.OrderByDescending(lr => lr.ToDate).FirstOrDefault()?.ToDate,
                CompletedDays = completedDays,
                RemainingDays = remainingDays,
                PhoneNumber = employee.Phone ?? string.Empty,
                Status = employee.Status.StatusText ?? string.Empty,

                TeacherPopUpDTO = new TeacherPopUpDTO
                {
                    Photo = employee.Photo != null ? employee.Photo.PhotoImageName : null,
                    Name = $"{employee.FirstName ?? string.Empty} {employee.LastName ?? string.Empty}",
                    Subject = employee.Subject?.SubjectText ?? string.Empty,
                    SchoolName = employee.School != null
            ? $"{employee.School.SchoolName ?? string.Empty}, " +
              $"{employee.School.Address ?? string.Empty}, " +
              $"{employee.School.City?.CityName ?? string.Empty}, " +
              $"{employee.School.State ?? string.Empty}, " +
              $"{employee.School.Pincode ?? string.Empty}"
            : string.Empty,
                    Email = employee.Email ?? string.Empty,
                    PhoneNumber = employee.Phone ?? string.Empty,
                    DateofJoin = employee.HireDate,
                    ReportedTo = employee.Supervisor != null
            ? $"{employee.Supervisor.FirstName ?? string.Empty} {employee.Supervisor.LastName ?? string.Empty}"
            : string.Empty,
                }
            };
        }


    }
}

