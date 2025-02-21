using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.ModelDTO.CourseDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using System.Text.RegularExpressions;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using Microsoft.Extensions.Logging;
using System.Collections;
using Hangfire;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TeachersApp.Entity.ModelDTO.PromotionDTO;

namespace TeachersApp.Services.Repositories
{
    public class TeacherService : ITeacherService
    {
        private readonly TeachersAppDbcontext _context;
        private readonly IPersonalDetailsService _personalDetailsService;
        private readonly IPromotionService _promotionService;
        private readonly ITeacherHistoryService _historyService;
        private readonly ILogger<TeacherService> _logger;


        public TeacherService(TeachersAppDbcontext context, IPersonalDetailsService personalDetailsService, ILogger<TeacherService> logger, IPromotionService promotionService, ITeacherHistoryService teacherHistoryService)
        {
            _context = context;
            _personalDetailsService = personalDetailsService;
            _logger = logger;
            _promotionService = promotionService;
            _historyService = teacherHistoryService;

        }

        // Get Total Active Teacher Count

        #region GetTotalActiveTeachersCount

        public async Task<int> GetTotalActiveTeachersCountAsync()
        {
            return await _context.Employees.Where(s => s.Status.StatusText == "Active" && s.Status.StatusType == "Employee" && s.EmployeeType.Employeetype == "Teaching Staff") // Accessing StatusText through the navigation property
                .CountAsync();
        }

        #endregion




        #region GetPromotionEligibleTeachersCountAsync

        public async Task<int> GetPromotionEligibleTeachersCountAsync()
        {
            return await _context.Employees.Where(s => s.PromotionEligible == true && s.Status.StatusText == "Active" && s.Status.StatusType == "Employee") // Accessing StatusText through the navigation property
                .CountAsync();
        }

        #endregion







        // Completed --

        #region GetEmployeeList

        public async Task<IEnumerable<Employee>> GetListTeachersDataAsync()
        {
            try
            {
                var teacherEntities = await _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.School)
                        .ThenInclude(s => s.City)
                    .Include(e => e.Supervisor)
                    .Include(e => e.Photo)
                    .Include(e => e.TeacherHistories)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                    .Where(e => e.Status != null &&
                                e.Status.StatusText == "Active" &&
                                e.Status.StatusType == "Employee" &&
                                e.EmployeeType != null &&
                                e.EmployeeType.Employeetype == "Teaching Staff")
                    .ToListAsync();

                if (teacherEntities == null || !teacherEntities.Any())
                {
                    throw new ArgumentException("No active employees found.");
                }

                return teacherEntities;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving employees.", ex);
            }
        }





        #endregion
        public async Task<IEnumerable<TeacherListDTO>> GetFilterListTeachersDataAsync(
            int? subjectId = null,
            int? retiringInMonths = null,
            int? schoolId = null,
            string? uniqueIdFilter = null,
            bool? DocumentsWithError = null,
            int? minExperienceYear = null,
            int? maxExperienceYear = null)
        {
            var currentDate = DateTime.Now;
            DateTime? retirementDateThreshold = retiringInMonths.HasValue ? currentDate.AddMonths(retiringInMonths.Value) : (DateTime?)null;

            // Build the base query
            var query = _context.Employees
               .Include(e => e.Designation)
                    .Include(t => t.School)
                    .ThenInclude(t => t.City)
                    .Include(t => t.Supervisor)
                    .Include(t => t.Photo)
                    .Include(t => t.TeacherHistories)
                    .Include(e => e.School)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
               .Where(e => e.Status != null &&
                            e.Status.StatusText == "Active" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType.Employeetype != null &&
                            e.EmployeeType.Employeetype == "Teaching Staff"); // Filter for Teaching Staff

            // Apply Subject Filter
            if (subjectId.HasValue)
            {
                query = query.Where(e => e.SubjectID != null && e.SubjectID == subjectId);
            }

            // Apply School Filter
            if (schoolId.HasValue)
            {
                query = query.Where(e => e.School != null && e.SchoolID == schoolId);
            }

            // Apply School Filter
            if (!string.IsNullOrEmpty(uniqueIdFilter))
            {
                query = query.Where(e => e.UniqueID != null && e.UniqueID == uniqueIdFilter);
            }

            // Apply Retirement Date Filter
            if (retirementDateThreshold.HasValue)
            {
                query = query.Where(e => e.RetirementDate.HasValue
            && e.RetirementDate.Value <= retirementDateThreshold.Value
            && e.RetirementDate.Value > currentDate);
            }

            // Apply Document With Error Filter
            if (DocumentsWithError.HasValue)
            {
                query = query.Where(e => e.EmployeeEducations.Any(ee =>
                    ee.Document != null &&
                    ee.Document.Status != null &&
                    ((DocumentsWithError.Value && ee.Document.Status.StatusText == "Pending") ||
                     (!DocumentsWithError.Value && ee.Document.Status.StatusText != "Pending"))));
            }

            // Retrieve data and apply in-memory filtering for experience if needed
            var employees = await query.ToListAsync();

            if (minExperienceYear.HasValue || maxExperienceYear.HasValue)
            {
                employees = employees
                    .Where(e =>
                        (!minExperienceYear.HasValue || CalculateExperience(e.WorkStartDate, currentDate) >= minExperienceYear.Value) &&
                        (!maxExperienceYear.HasValue || CalculateExperience(e.WorkStartDate, currentDate) <= maxExperienceYear.Value)
                    )
                    .ToList();
            }

            // Map to DTO and return
            return employees.Select(e => e.GetEmployeeFilterListDTO()).ToList();
        }

        public async Task<IEnumerable<Employee>> GetFilterDynamicListTeachersDataAsync(
     int? subjectId = null,
     int? retiringInMonths = null,
     int? schoolId = null,
     string? uniqueId = null,
     int? minExperienceYear = null,
     int? maxExperienceYear = null,
     List<string>? statusTexts = null,
     int? additionalSchoolId = null)
        {
            var currentDate = DateTime.Now;
            DateTime? retirementDateThreshold = retiringInMonths.HasValue ? currentDate.AddMonths(retiringInMonths.Value) : (DateTime?)null;

            // Base query
            var query = _context.Employees
                .Include(e => e.Designation)
                .Include(e => e.School)
                .Include(e => e.Subject)
                .Include(e => e.Status)
                .Where(e => e.EmployeeType != null && e.EmployeeType.Employeetype == "Teaching Staff");

            // Apply filters
            if (subjectId.HasValue)
                query = query.Where(e => e.SubjectID != null && e.SubjectID == subjectId);

            if (schoolId.HasValue)
                query = query.Where(e => e.SchoolID != null && e.SchoolID == schoolId);

            if (additionalSchoolId.HasValue)
                query = query.Where(e => e.SchoolID != null && e.SchoolID == additionalSchoolId); // FIXED: using correct parameter

            if (!string.IsNullOrEmpty(uniqueId))
                query = query.Where(e => e.UniqueID == uniqueId);

            if (retirementDateThreshold.HasValue)
                query = query.Where(e => e.RetirementDate.HasValue &&
                                         e.RetirementDate.Value <= retirementDateThreshold.Value &&
                                         e.RetirementDate.Value > currentDate);

            // Apply status filter only if valid statuses are provided
            if (statusTexts != null && statusTexts.Any())
            {
                query = query.Where(e => e.Status != null && statusTexts.Contains(e.Status.StatusText));
            }

            // Execute database query
            var employees = await query.ToListAsync();

            if (minExperienceYear.HasValue || maxExperienceYear.HasValue)
            {
                employees = employees
                    .Where(e =>
                        (minExperienceYear == null || minExperienceYear == 0 || CalculateExperience(e.HireDate, currentDate) >= minExperienceYear.Value) &&
                        (!maxExperienceYear.HasValue || CalculateExperience(e.HireDate, currentDate) <= maxExperienceYear.Value))
                    .ToList();
            }


            return employees;
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





        // Completed --


        #region GetTeacherPopUp


        public async Task<TeacherPopUpDTO> GetTeacherPopUpAsync(int teacherId)
        {
            try
            {
                var teacher = await _context.Employees
                .Include(t => t.School)
                .ThenInclude(t => t.City)
                .Include(t => t.Supervisor)
                .Include(t => t.Subject)
                .Include(t => t.Photo)
                .Include(t => t.TeacherHistories)
                .FirstOrDefaultAsync(s => s.EmployeeID == teacherId);
                if (teacher == null)
                {
                    // Handle case where school is not found
                    throw new ArgumentException($"Employee with ID {teacher} not found.");
                }

                // Map School entity to GetSchoolDTO using extension method
                return teacher.ToGetTeacherPopUpDTO();
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving Employee details.", ex);
            }
        }
        #endregion










        public async Task<Employee> CreateEmployeesAsync(Employee employee, CreateEmployeeDTO employeeDto)
        {
            if (employee.WorkStartDate == null)
                throw new ArgumentException("WorkStartDate is required to generate a unique ID.");
          
            var availablePosition = await _context.SchoolPositions
       .Where(sp => sp.SchoolID == employee.SchoolID
                    && sp.DesignationID == employee.DesignationID
                    && sp.StatusID == 7 || sp.StatusID == 8)
       .FirstOrDefaultAsync();

            if (availablePosition == null)
            {

                return null;
            }
            employee.SchoolPositionID = availablePosition.PositionID;
            // Fetch Employee Type
            string employeeType = await _context.EmployeeTypes
                .Where(et => et.EmployeeTypeID == employee.EmployeeTypeID)
                .Select(et => et.Employeetype)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(employeeType))
                throw new ArgumentException("Invalid EmployeeTypeID.");

            // Generate Unique Employee ID
            string uniqueId = await GenerateUniqueEmployeeIDAsync(employeeType, employee.WorkStartDate.Value);
            employee.UniqueID = uniqueId;

            // Assign SupervisorID (School Principal)
            employee.SupervisorID = await _context.Schools
                .Where(s => s.SchoolID == employee.SchoolID)
                .Select(s => s.PrincipalID)
                .FirstOrDefaultAsync();

            // Assign Employee Status
            employee.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "New" && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            // Validation for Non-Teaching Staff
            int? nonTeachingStaffId = await _context.EmployeeTypes
                .Where(et => et.Employeetype == "Non-Teaching Staff")
                .Select(et => et.EmployeeTypeID)
                .FirstOrDefaultAsync();

            int? officeStaffDesignationId = await _context.Designations
                .Where(d => d.DesignationText == "Office Staff")
                .Select(d => d.DesignationID)
                .FirstOrDefaultAsync();

            if (employee.EmployeeTypeID == nonTeachingStaffId && employee.DesignationID != officeStaffDesignationId)
            {
                throw new ArgumentException("Only 'Office Staff' designation is allowed for Non-Teaching Staff.");
            }


            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync(); // Ensure EmployeeID is assigned

            // Ensure EmployeeID is Set Before Proceeding
            if (employee.EmployeeID == 0 || employee.EmployeeID == null)
            {
                throw new Exception("EmployeeID was not generated correctly.");
            }

            // Fetch RoleIDs
            int? employeeRoleId = await _context.Roles
                .Where(r => r.RoleName == "Employee")
                .Select(r => r.RoleID)
                .FirstOrDefaultAsync();

            int? headmasterRoleId = await _context.Roles
                .Where(r => r.RoleName == "Head Master")
                .Select(r => r.RoleID)
                .FirstOrDefaultAsync();

            // Determine RoleID based on EmployeeTypeID and DesignationID
            int? assignedRoleId;

            // Check if the Designation is "Head Master"
            bool isHeadMaster = await _context.Designations
                .Where(d => d.DesignationID == employee.DesignationID && d.DesignationText == "Head Master")
                .AnyAsync();

            if (isHeadMaster)
            {
                assignedRoleId = headmasterRoleId;
            }
            else
            {
                // Default to "Employee" role if no special designation is found
                assignedRoleId = employeeRoleId;
            }

            if (assignedRoleId == null)
            {
                throw new Exception("Role ID not found for the given Employee Type or Designation.");
            }



            // Hash the Password before saving the User record
            string hashedPassword = HashPassword(employeeDto.PasswordHash);



            // Create a User record for the employee
            var user = new User
            {
                Username = employee.FirstName,
                PasswordHash = hashedPassword,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateOfBirth = employee.DateOfBirth ?? DateTime.UtcNow,
                EmployeeID = employee.EmployeeID,
                RoleID = assignedRoleId.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
           
            availablePosition.StatusID = 9;
            _context.SchoolPositions.Update(availablePosition);
            await _context.SaveChangesAsync();

            // Now call dependent methods that require EmployeeID
            employee.PromotionEligible = await _promotionService.PromotionEligible(employee);
            await _historyService.HandleTeacherHistoryAsync(employee);
            // Save changes again if PromotionEligible affects the database
            await _context.SaveChangesAsync();

            return employee;
        }

        private string HashPassword(string password)
        {
            // Use a strong hash function such as PBKDF2 with a unique salt
            var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{salt}:{hashed}";
        }

        public async Task<string> GenerateUniqueEmployeeIDAsync(string employeeType, DateTime workStartDate)
        {
            var year = workStartDate.Year;

            var generatedIdParam = new SqlParameter
            {
                ParameterName = "@GeneratedID",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC GenerateUniqueEmployeeID @EmployeeType = {0}, @WorkStartYear = {1}, @GeneratedID = @GeneratedID OUTPUT",
                employeeType, year, generatedIdParam);

            return (string)generatedIdParam.Value;
        }


        public async Task<List<AddEmployeeCourseDTO>> GetCoursesByEducationTypeAsync(int educationTypeId)
        {


            var courses = await _context.Courses
                .Where(c => c.EducationTypeID == educationTypeId)
                .Select(c => new AddEmployeeCourseDTO
                {
                    CourseID = c.CourseID,
                    CourseName = c.CourseName
                })
                .ToListAsync();

            return (courses);
        }


        #region TeacherDocumentUpload




        #endregion


        public async Task<Employee> GetEmployeeByUniqueIDAsync(string UniqueID)
        {
            var employee = await _context.Employees
                 .Include(e => e.Designation)
                 .Include(e => e.EmployeeType)
                 .Include(e => e.EmployeeCategory)
                 .Include(e => e.School)
                 .Include(e => e.Photo)
                 .Include(e => e.Subject)
                 .Include(e => e.SchoolsPosition)
                 .Include(e => e.Status)
                 .Include(e => e.ApprovalType)
                 .Include(e => e.Supervisor)
                 .Include(e => e.EmployeeEducations)
                     .ThenInclude(ee => ee.Course)
                 .Include(e => e.EmployeeEducations)
                     .ThenInclude(ee => ee.Document)
                     .Include(e => e.EmployeeDocuments)
                    .ThenInclude(ee => ee.Document)
                 .Include(e => e.PersonalDetails)
                     .ThenInclude(pd => pd.Sex)
                 .Include(e => e.PersonalDetails.EmployeeReligion)
                 .Include(e => e.PersonalDetails.EmployeeSpouseReligion)
                 .Include(e => e.PersonalDetails.CasteCategory)
                 .Include(e => e.PersonalDetails.BloodGroup)
                 .Include(e => e.PersonalDetails.MaritalStatus)
                 .Include(e => e.PersonalDetails.District)
                 .FirstOrDefaultAsync(e => e.UniqueID == UniqueID);

            if (employee == null) return null;

            return employee;
        }
        public async Task<Employee?> UpdateEmployeeAsync(int Id, Employee updatedEmployee)
        {
            // Retrieve the existing employee from the database by ID
            var existingEmployee = await _context.Employees
                .Include(e => e.PersonalDetails)
                .Include(e => e.EmployeeDocuments)
                .Include(e => e.EmployeeEducations)
                .FirstOrDefaultAsync(e => e.EmployeeID == Id)
                .ConfigureAwait(false);

            // Fetch StatusID based on Designation
            int hmDesignationId = await _context.Designations
                .Where(d => d.DesignationText == "Head Master")
                .Select(d => d.DesignationID)
                .FirstOrDefaultAsync();

            int newStatusID = await _context.Statuses
                .Where(s => s.StatusText == "New" && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            int pendingStatusID = await _context.Statuses
                .Where(s => s.StatusText == "Pending" && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            int statusID = existingEmployee.StatusID ?? throw new InvalidOperationException("StatusID cannot be null.");

            // If designation is Head Master, set the status to "HMApproved"
            if (updatedEmployee.DesignationID == hmDesignationId)
            {
                statusID = await _context.Statuses
                    .Where(s => s.StatusText == "HMApproved" && s.StatusType == "Employee")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();
            }
            // Check if the employee's current status is "New"
            if (existingEmployee.StatusID == newStatusID)
            {
                {
                    // If status is "New", update it to "Pending"
                    statusID = pendingStatusID;
                }
            }

            if (statusID == 0)
            {
                throw new InvalidOperationException("Required status for Employee not found in the database.");
            }

            if (existingEmployee == null)
            {
                return null; // Employee not found
            }

            // Update employee properties
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Phone = updatedEmployee.Phone;
            existingEmployee.PresentAddress = updatedEmployee.PresentAddress;
            existingEmployee.PermanentAddress = updatedEmployee.PermanentAddress;
            existingEmployee.PhotoID = updatedEmployee.PhotoID;
            existingEmployee.DateOfBirth = updatedEmployee.DateOfBirth;
            existingEmployee.WorkStartDate = updatedEmployee.WorkStartDate;
            existingEmployee.HireDate = updatedEmployee.HireDate;
            existingEmployee.RetirementDate = updatedEmployee.RetirementDate;
            existingEmployee.DesignationID = updatedEmployee.DesignationID;
            existingEmployee.CategoryID = updatedEmployee.CategoryID;
            existingEmployee.SchoolID = updatedEmployee.SchoolID;
            existingEmployee.SubjectID = updatedEmployee.SubjectID;
            existingEmployee.SchoolPositionID = updatedEmployee.SchoolPositionID;
            existingEmployee.ApprovalTypeID = updatedEmployee.ApprovalTypeID;
            existingEmployee.ApprovalTypeReason = updatedEmployee.ApprovalTypeReason;
            existingEmployee.StatusID = statusID;
            existingEmployee.UpdatedAt = DateTime.Now;

            // Update personal details if they exist
            if (existingEmployee.PersonalDetails != null && updatedEmployee.PersonalDetails != null)
            {
                existingEmployee.PersonalDetails.PEN = updatedEmployee.PersonalDetails.PEN;
                existingEmployee.PersonalDetails.SexID = updatedEmployee.PersonalDetails.SexID;
                existingEmployee.PersonalDetails.ReligionID = updatedEmployee.PersonalDetails.ReligionID;
                existingEmployee.PersonalDetails.CasteID = updatedEmployee.PersonalDetails.CasteID;
                existingEmployee.PersonalDetails.Caste = updatedEmployee.PersonalDetails.Caste;
                existingEmployee.PersonalDetails.BloodGroupID = updatedEmployee.PersonalDetails.BloodGroupID;
                existingEmployee.PersonalDetails.DifferentlyAbled = updatedEmployee.PersonalDetails.DifferentlyAbled;
                existingEmployee.PersonalDetails.ExServiceMen = updatedEmployee.PersonalDetails.ExServiceMen;
                existingEmployee.PersonalDetails.IdentificationMark1 = updatedEmployee.PersonalDetails.IdentificationMark1;
                existingEmployee.PersonalDetails.IdentificationMark2 = updatedEmployee.PersonalDetails.IdentificationMark2;
                existingEmployee.PersonalDetails.Height = updatedEmployee.PersonalDetails.Height;
                existingEmployee.PersonalDetails.FatherName = updatedEmployee.PersonalDetails.FatherName;
                existingEmployee.PersonalDetails.MotherName = updatedEmployee.PersonalDetails.MotherName;
                existingEmployee.PersonalDetails.InterReligion = updatedEmployee.PersonalDetails.InterReligion;
                existingEmployee.PersonalDetails.MaritalStatusID = updatedEmployee.PersonalDetails.MaritalStatusID;
                existingEmployee.PersonalDetails.SpouseName = updatedEmployee.PersonalDetails.SpouseName;
                existingEmployee.PersonalDetails.SpouseReligionID = updatedEmployee.PersonalDetails.SpouseReligionID;
                existingEmployee.PersonalDetails.SpouseCaste = updatedEmployee.PersonalDetails.SpouseCaste;
                existingEmployee.PersonalDetails.PanID = updatedEmployee.PersonalDetails.PanID;
                existingEmployee.PersonalDetails.VoterID = updatedEmployee.PersonalDetails.VoterID;
                existingEmployee.PersonalDetails.AadhaarID = updatedEmployee.PersonalDetails.AadhaarID;
                existingEmployee.PersonalDetails.PFNummber = updatedEmployee.PersonalDetails.PFNummber;
                existingEmployee.PersonalDetails.PRAN = updatedEmployee.PersonalDetails.PRAN;
                existingEmployee.PersonalDetails.DistrictID = updatedEmployee.PersonalDetails.DistrictID;
                existingEmployee.PersonalDetails.EligibilityTestQualified = updatedEmployee.PersonalDetails.EligibilityTestQualified;
                existingEmployee.PersonalDetails.ProtectedTeacher = updatedEmployee.PersonalDetails.ProtectedTeacher;
            }

            if (updatedEmployee.EmployeeEducations != null)
            {
                // Remove existing educations
                if (existingEmployee.EmployeeEducations != null && existingEmployee.EmployeeEducations.Any())
                {
                    foreach (var education in existingEmployee.EmployeeEducations.ToList())
                    {
                        _context.EmployeeEducations.Remove(education);
                    }
                }

                // Add the new ones
                foreach (var education in updatedEmployee.EmployeeEducations)
                {
                    existingEmployee.EmployeeEducations.Add(new EmployeeEducation
                    {
                        CourseID = education.CourseID,
                        CourseName = education.CourseName,
                        University = education.University,
                        FromDate = education.FromDate,
                        ToDate = education.ToDate,
                        DocumentID = education.DocumentID
                    });
                }
            }

            if (updatedEmployee.EmployeeDocuments != null)
            {
                // If the list is empty, remove all existing documents
                if (!updatedEmployee.EmployeeDocuments.Any())
                {
                    if (existingEmployee.EmployeeDocuments != null && existingEmployee.EmployeeDocuments.Any())
                    {
                        foreach (var document in existingEmployee.EmployeeDocuments.ToList())
                        {
                            _context.EmployeeDocuments.Remove(document);
                        }
                    }
                }
                else
                {
                    // If the list contains documents, replace the old ones
                    if (existingEmployee.EmployeeDocuments != null && existingEmployee.EmployeeDocuments.Any())
                    {
                        foreach (var document in existingEmployee.EmployeeDocuments.ToList())
                        {
                            _context.EmployeeDocuments.Remove(document);
                        }
                    }

                    // Add the new documents
                    foreach (var document in updatedEmployee.EmployeeDocuments)
                    {
                        existingEmployee.EmployeeDocuments.Add(new EmployeeDocument
                        {
                            DocumentID = document.DocumentID
                        });
                    }
                }
            }

            // Update associated User table if applicable
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeID == existingEmployee.EmployeeID);
            if (user != null)
            {
                user.FirstName = existingEmployee.FirstName ?? string.Empty;
                user.LastName = existingEmployee.LastName ?? string.Empty;
                user.DateOfBirth = existingEmployee.DateOfBirth;
                user.UpdatedAt = DateTime.Now;
                // Check if the Designation is updated to Headmaster
                var headmasterDesignationId = await _context.Designations
                    .Where(d => d.DesignationText == "Head Master")
                    .Select(d => d.DesignationID)
                    .FirstOrDefaultAsync();

                if (existingEmployee.DesignationID != headmasterDesignationId && updatedEmployee.DesignationID == headmasterDesignationId)
                {
                    var headmasterRoleId = await _context.Roles
                        .Where(r => r.RoleName == "Head Master")
                        .Select(r => r.RoleID)
                        .FirstOrDefaultAsync();

                    if (headmasterRoleId > 0)
                    {
                        user.RoleID = headmasterRoleId;
                    }
                }

                _context.Users.Update(user);
            }


            // Save changes to the database
            await _context.SaveChangesAsync().ConfigureAwait(false);

            // Update PromotionEligible after saving changes
            existingEmployee.PromotionEligible = await _promotionService.PromotionEligible(existingEmployee);

            // Save the updated PromotionEligible status
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return existingEmployee;
        }


        public async Task<Employee?> GetEmployeeByIdAsync(int EmployeeID)
        {
            var employee = await _context.Employees
                .Include(e => e.Designation)
                .Include(e => e.EmployeeType)
                .Include(e => e.EmployeeCategory)
                .Include(e => e.School)
                .ThenInclude(s => s.City)
                .Include(e => e.Photo)
                .Include(e => e.Subject)
                .Include(e => e.SchoolsPosition)
                .Include(e => e.Status)
                .Include(e => e.ApprovalType)
                .Include(e => e.Supervisor)
                .Include(e => e.EmployeeEducations)
                .ThenInclude(ee => ee.Course)
                .ThenInclude(c => c.EducationType)
                .Include(e => e.EmployeeEducations)
                    .ThenInclude(ee => ee.Course)
                .Include(e => e.EmployeeEducations)
                    .ThenInclude(ee => ee.Document)
                .Include(e => e.EmployeeDocuments)
                    .ThenInclude(ee => ee.Document)
                .Include(e => e.PersonalDetails)
                    .ThenInclude(pd => pd.Sex)
                .Include(e => e.PersonalDetails.EmployeeReligion)
                .Include(e => e.PersonalDetails.EmployeeSpouseReligion)
                .Include(e => e.PersonalDetails.CasteCategory)
                .Include(e => e.PersonalDetails.BloodGroup)
                .Include(e => e.PersonalDetails.MaritalStatus)
                .Include(e => e.PersonalDetails.District)
                .FirstOrDefaultAsync(c => c.EmployeeID == EmployeeID);

            if (employee == null) return null;

            return employee;

        }


        public async Task<List<Employee>> GetAllRetiredTeacherAsync()
        {
            return await _context.Employees
             .Include(e => e.Designation)
                    .Include(t => t.School)
                    .ThenInclude(t => t.City)
                    .Include(t => t.Supervisor)
                    .Include(t => t.Photo)
                    .Include(t => t.TeacherHistories)
                    .Include(e => e.School)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                   .Where(e => e.Status != null &&
                            e.Status.StatusText == "Active" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType.Employeetype != null &&
                            e.EmployeeType.Employeetype == "Teaching Staff" && e.RetirementDate <= DateTime.Now) // Filter for Teaching Staff
                .ToListAsync();
        }


        public async Task<List<Employee>> GetAllTeacherOnLeaveAsync()
        {
            return await _context.Employees
             .Include(e => e.Designation)
                    .Include(t => t.School)
                    .ThenInclude(t => t.City)
                    .Include(t => t.Supervisor)
                    .Include(t => t.Photo)
                    .Include(t => t.TeacherHistories)
                    .Include(t => t.LeaveRequests)
                    .Include(e => e.School)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                   .Where(e => e.Status != null &&
                            e.Status.StatusText == "Leave" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType.Employeetype != null &&
                            e.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesBySchoolIDAndSchoolTypeIdAndEmployeeTypeIDAsync(
             int? schoolID = null,
             List<int?> schooltypeIDs = null,
             List<int?> employeeTypeIDs = null)
        {
            var query = _context.Employees
                .Include(e => e.Designation)
                .Include(e => e.School)
                    .ThenInclude(s => s.City)
                .Include(e => e.Supervisor)
                .Include(e => e.Photo)
                .Include(e => e.EmployeeType)
                .Include(e => e.Subject)
                .Where(e => e.Status != null &&
                             e.Status.StatusText != "Retired" &&
                            e.Status.StatusType == "Employee");

            if (employeeTypeIDs != null && employeeTypeIDs.Any())
            {
                query = query.Where(e => employeeTypeIDs.Contains(e.EmployeeTypeID));
            }

            if (schoolID.HasValue)
            {
                query = query.Where(e => e.School.SchoolID == schoolID.Value);
            }

            if (schooltypeIDs != null && schooltypeIDs.Any())
            {
                query = query.Where(e => e.Designation.SchoolTypeDesignations
                                               .Any(std => schooltypeIDs.Contains(std.SchoolTypeID)));
            }




            // Apply sorting based on the provided parameters
            if (schoolID.HasValue || schooltypeIDs != null || employeeTypeIDs != null)
            {
                // Step 1: Order teachers with specific designations (Principal, Vice Principal, Head Master) first
                query = query.OrderBy(e => e.Designation.DesignationText == "Principal" ? 0 :
                                           e.Designation.DesignationText == "Vice Principal" ? 1 :
                                           e.Designation.DesignationText == "Head Master" ? 2 : 3)
                             .ThenBy(e => e.HireDate);  // Step 2: Then, order by HireDate for all employees

                // Step 3: If Employee Type is "Non - Teaching Staff", don't sort by Designation, just HireDate
                if (employeeTypeIDs != null && employeeTypeIDs.Any() &&
                !employeeTypeIDs.Contains(_context.EmployeeTypes
                                       .Where(et => et.Employeetype == "Teaching Staff")
                                       .Select(et => et.EmployeeTypeID)
                                       .FirstOrDefault()))  // Assuming EmployeeType is "Teaching Staff"
                {
                    query = query.OrderBy(e => e.HireDate);
                }
            }

            return await query.ToListAsync();

        }

        public async Task<List<Employee>> GetTeachersBySchoolIDAsync(int schoolID)
        {
            var schoolExists = await _context.Schools.AnyAsync(s => s.SchoolID == schoolID);
            if (!schoolExists)
            {
                throw new KeyNotFoundException($"School with ID {schoolID} does not exist.");
            }
            return await _context.Employees
                .Include(e => e.Designation)
                .Include(e => e.School)
                    .ThenInclude(s => s.City)
                .Include(e => e.Supervisor)
                .Include(e => e.Photo)
                .Include(e => e.TeacherHistories)
                .Include(e => e.EmployeeType)
                .Include(e => e.Subject)
                .Include(e => e.Promotions)
                .Include(e => e.EmployeeEducations)
                    .ThenInclude(ee => ee.Document)
                    .ThenInclude(d => d.Status)
                .Where(e => e.SchoolID == schoolID &&
                            e.Status != null &&
                            e.Status.StatusText == "Active" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType != null &&
                            e.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task ProcessRetirementsAsync()
        {
            try
            {
                var today = DateTime.Today;

                // Get employees retiring today
                var retiringEmployees = await _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.School)
                    .Where(e => e.RetirementDate.HasValue && e.RetirementDate.Value.Date == today)
                    .ToListAsync();

                foreach (var employee in retiringEmployees)
                {
                    try
                    {
                        // Add entry to TeacherHistory
                        var retirementHistory = new TeacherHistory
                        {
                            EmployeeID = employee.EmployeeID,
                            ChangeDate = today,
                            ChangeDescription = $"Retired from {employee.Designation?.DesignationText ?? "Unknown"}",
                            ChangeTypeID = await _historyService.GetChangeTypeIdAsync("Retired"),
                            ChangeFromSchoolID = employee.SchoolID,
                            ChangeToSchoolID = null,
                            PromotedFromPositionID = employee.DesignationID,
                            PromotedToPositionID = null
                        };

                        await _historyService.CreateTeacherHistoryAsync(retirementHistory);

                        // Update employee status to "Retired"
                        employee.StatusID = await _context.Statuses
                            .Where(s => s.StatusText == "Retired" && s.StatusType == "Employee")
                            .Select(s => s.StatusID)
                            .FirstOrDefaultAsync();

                        _logger.LogInformation($"Processed retirement for EmployeeID: {employee.EmployeeID}, " +
                                               $"Name: {employee.FirstName} {employee.LastName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing retirement for EmployeeID: {employee.EmployeeID}. Exception: {ex.Message}");
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ProcessRetirementsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Employee>> GetPromotionEligibleEmployeeList()
        {
            try
            {
                var promotionEligibleEmployees = await _context.Employees

                    .Where(e => e.Status != null &&
                                e.Status.StatusText == "Active" &&
                                e.Status.StatusType == "Employee" &&
                                e.EmployeeType != null &&
                                e.EmployeeType.Employeetype == "Teaching Staff" &&
                                e.PromotionEligible == true)
                    .ToListAsync();


                return promotionEligibleEmployees;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving employees.", ex);
            }
        }

        public async Task<List<Employee>> GetApprovedTeacherAsync()
        {
            return await _context.Employees
             .Include(e => e.Designation)
                    .Include(t => t.School)
                    .ThenInclude(t => t.City)
                    .Include(t => t.Supervisor)
                    .Include(t => t.Photo)
                    .Include(t => t.TeacherHistories)
                    .Include(e => e.School)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                   .Where(e => e.Status != null &&
                            e.Status.StatusText == "Active" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType.Employeetype != null &&
                            e.EmployeeType.Employeetype == "Teaching Staff" &&
                            e.ApprovalType != null &&
                            e.ApprovalType.Approvaltype == "Approved")
                .ToListAsync();
        }

        public async Task<List<Employee>> GetNonApprovedTeacherAsync()
        {
            return await _context.Employees
             .Include(e => e.Designation)
                    .Include(t => t.School)
                    .ThenInclude(t => t.City)
                    .Include(t => t.Supervisor)
                    .Include(t => t.Photo)
                    .Include(t => t.TeacherHistories)
                    .Include(e => e.School)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                   .Where(e => e.Status != null &&
                            e.Status.StatusText == "Active" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType.Employeetype != null &&
                            e.EmployeeType.Employeetype == "Teaching Staff" &&
                            e.ApprovalType != null &&
                            e.ApprovalType.Approvaltype == "Non-Approved")
                .ToListAsync();
        }

        public async Task<Employee?> GetPromotedDesignationByEmployeeIDAsync(int employeeID)
        {
            // Retrieve the employee along with their current designation
            var employee = await _context.Employees
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(e => e.EmployeeID == employeeID);

            if (employee == null || employee.Designation == null)
            {
                return null; // No employee or designation found
            }

            // Find the next designation based on promotion hierarchy
            var nextDesignation = await _context.Designations
                .Where(d => d.DesignationID > employee.DesignationID) // Get the next higher designation
                .OrderBy(d => d.DesignationID) // Ensure it's the immediate next
                .FirstOrDefaultAsync();

            if (nextDesignation == null)
            {
                return null; // No higher designation found
            }

            // Create a new employee object with the updated designation
            var promotedEmployee = new Employee
            {
                DesignationID = nextDesignation.DesignationID, // Assign next designation
                Designation = nextDesignation, // Assign new designation
                                               // Copy other properties from employee as needed
            };

            return promotedEmployee;
        }

        public async Task<IEnumerable<Employee>> GetDynamicListTeachersDataAsync(List<string>? statusTexts, int? schoolId)
        {
            try
            {
                var query = _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.School)
                        .ThenInclude(s => s.City)
                    .Include(e => e.Supervisor)
                    .Include(e => e.Photo)
                    .Include(e => e.TeacherHistories)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                    .Where(e => e.EmployeeType != null && e.EmployeeType.Employeetype == "Teaching Staff");

                // Apply status filter only if statuses are provided
                if (statusTexts != null && statusTexts.Any())
                {
                    query = query.Where(e => e.Status != null && statusTexts.Contains(e.Status.StatusText));
                }

                // Apply school filter only if schoolId is provided
                if (schoolId.HasValue)
                {
                    query = query.Where(e => e.SchoolID == schoolId.Value);
                }

                var teacherEntities = await query.ToListAsync();
                return teacherEntities;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving employees.", ex);
            }
        }


        public async Task<Employee?> ApproveEmployeeByHeadMasterAsync(int Id)
        {
            // Find the employee by ID
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeID == Id);

            if (employee == null)
            {
                return null; // Employee not found
            }

            // Find the status ID for "HMApproved"
            var status = await _context.Statuses
                .FirstOrDefaultAsync(s => s.StatusText == "HMApproved" && s.StatusType == "Employee");

            if (status == null) { return null; }

            if (employee.StatusID == status.StatusID) { throw new Exception("Employee already has HMApproved Status"); }

            // Update the employee's StatusID
            employee.StatusID = status.StatusID;

            // Save changes to the database
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return employee; // Return the updated employee
        }


        public async Task<Employee?> ApproveEmployeeByManagerAsync(int Id)
        {
            // Find the employee by ID
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeID == Id);

            if (employee == null)
            {
                return null; // Employee not found
            }

            // Find the status ID for "HMApproved"
            var status = await _context.Statuses
                .FirstOrDefaultAsync(s => s.StatusText == "Active" && s.StatusType == "Employee");

            if (status == null)
            {
                return null; // Status not found
            }

            if (employee.StatusID == status.StatusID) { throw new Exception("Employee already has Active Status"); }

            // Update the employee's StatusID
            employee.StatusID = status.StatusID;

            // Save changes to the database
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return employee; // Return the updated employee
        }

        public async Task<TeacherStatusCountDTO> GetTotalTeachersStatusCountAsync()
        {
            return new TeacherStatusCountDTO
            {
                Active = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "Active" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Teaching Staff"),

                New = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "New" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Teaching Staff"),

                Pending = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "Pending" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Teaching Staff"),

                HMApproved = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "HMApproved" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Teaching Staff")
            };
        }


        public async Task<List<Employee>> GetEmployeeOrderByPromotionSeniorityBySchoolIDAsync(int schoolID)
        {
            try
            {
                // Fetch all employees matching the designationId and subjectId
                var employees = await _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.Subject)
                    .Include(e => e.School)
                    .Where(e => e.SchoolID == schoolID
                     && e.PromotionEligible == true
                     && e.PromotionSeniorityNumber != null)
                    .OrderBy(e => e.PromotionSeniorityNumber)
                    .ToListAsync();

                return employees;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving employee promotion list.", ex);
            }

        }


        public async Task<List<Employee>> GetOnLeaveTeachersBySchoolIDAsync(int schoolID)
        {
            try
            {
    
                bool schoolExists = await _context.Schools.AnyAsync(s => s.SchoolID == schoolID);
                if (!schoolExists)
                {
                    return null; 
                }

                return await _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.School)
                        .ThenInclude(s => s.City)
                    .Include(e => e.Supervisor)
                    .Include(e => e.Photo)
                    .Include(e => e.TeacherHistories)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.Subject)
                    .Include(e => e.Promotions)
                    .Include(e => e.Status)
                    .Where(e => e.SchoolID == schoolID &&
                                e.Status != null &&
                                e.Status.StatusText == "Leave" &&
                                e.Status.StatusType == "Employee" &&
                                e.EmployeeType != null &&
                                e.EmployeeType.Employeetype == "Teaching Staff")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving employees.", ex);
            }
        }
    }

}

