using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.NonTeacherDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TeachersApp.Services.Repositories
{
    public class NonTeacherService : INonTeacherInterface
    {
        private readonly TeachersAppDbcontext _context;
        private readonly IPromotionService _promotionService;
        private readonly ITeacherHistoryService _historyService;
        private readonly ILogger<NonTeacherService> _logger;

        public NonTeacherService (TeachersAppDbcontext context,ILogger<NonTeacherService> logger,IPromotionService promotionService,ITeacherHistoryService teacherHistoryService)
        {
            _context = context;
            _logger = logger;
            _promotionService = promotionService;
            _historyService = teacherHistoryService;
        }


        #region GetTotalActiveTeachersCount

        public async Task<int> GetTotalActiveNonTeachersCountAsync()
        {
            return await _context.Employees.Where(s => s.Status.StatusText == "Active" && s.Status.StatusType == "Employee" && s.EmployeeType.Employeetype == "Non-Teaching Staff") // Accessing StatusText through the navigation property
                .CountAsync();
        }

        #endregion

        public async Task<IEnumerable<NonTeacherList>> GetNonTeachersListAsync()
        {
            try
            {
                var teacherEntities = await _context.Employees
            .Include(e => e.Designation)
            .Include(e => e.School)
            .Include(e => e.EmployeeType)
            .Include(e => e.Status)
            .Where(e => e.Status != null &&
                 e.Status.StatusText == "Active" &&
                 e.Status.StatusType == "Employee" &&
                 e.EmployeeType != null && // Ensure EmployeeType is not null
                 e.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Non-Teaching Staff
            .ToListAsync();

                if (teacherEntities == null || !teacherEntities.Any())
                {
                    // Handle case where no schools are found
                    throw new ArgumentException("No Employee found.");
                }

                // Map School entities to GetAllSchoolsWithCityDTO using the extension method
                return teacherEntities.Select(teacherEntities => teacherEntities.GetNonTeacherEmployeeListDTO());
            }

            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving Employee.", ex);
            }

        }


        public async Task<IEnumerable<TeacherListDTO>> GetFilterListNonTeachersDataAsync(
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
                            e.EmployeeType.Employeetype == "Non-Teaching Staff"); // Filter for Teaching Staff

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

        public async Task<IEnumerable<Employee>> GetFilterDynamicListNonTeachersDataAsync(
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
                .Where(e => e.EmployeeType != null && e.EmployeeType.Employeetype == "Non-Teaching Staff");


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

            // Apply experience filtering in-memory
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

        public async Task<Employee> CreateNonTeacherAsync(Employee employee, CreateNonTeacherDTO employeeDto)
        {
            if (employee.WorkStartDate == null)
                throw new ArgumentException("WorkStartDate is required to generate a unique ID.");

            var availablePosition = await _context.SchoolPositions
       .Where(sp => sp.SchoolID == employee.SchoolID
                    && sp.DesignationID == employee.DesignationID
                    && (sp.StatusID == 7 || sp.StatusID == 8))
       .FirstOrDefaultAsync();

            if (availablePosition == null)
            {
                throw new ArgumentException("Position is not valid.");
            }

            string employeeType = await _context.EmployeeTypes
                .Where(et => et.EmployeeTypeID == employee.EmployeeTypeID)
                .Select(et => et.Employeetype)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(employeeType))
                throw new ArgumentException("Invalid EmployeeTypeID.");

            string uniqueId = await GenerateUniqueEmployeeIDAsync(employeeType, employee.WorkStartDate.Value);
            employee.UniqueID = uniqueId;

            employee.SupervisorID = await _context.Schools
                .Where(s => s.SchoolID == employee.SchoolID)
                .Select(s => s.PrincipalID)
                .FirstOrDefaultAsync();

            // Assign Employee Status
            employee.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "New" && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();


            if (employee.EmployeeTypeID == await _context.EmployeeTypes
                .Where(et => et.Employeetype == "Non-Teaching Staff")
                .Select(et => et.EmployeeTypeID)
                .FirstOrDefaultAsync() &&
                employee.DesignationID != await _context.Designations
                .Where(d => d.DesignationText == "Office Staff")
                .Select(d => d.DesignationID)
                .FirstOrDefaultAsync())
            {
                throw new ArgumentException("Only 'Office Staff' designation is allowed for Non-Teaching Staff.");
            }

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Ensure EmployeeID is Set Before Proceeding
            if (employee.EmployeeID == 0 || employee.EmployeeID == null)
            {
                throw new Exception("EmployeeID was not generated correctly.");
            }

            // Fetch RoleIDs
            int? teacherRoleId = await _context.Roles
                .Where(r => r.RoleName == "Employee")
                .Select(r => r.RoleID)
                .FirstOrDefaultAsync();

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
                RoleID = teacherRoleId.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await _historyService.HandleTeacherHistoryAsync(employee);

            availablePosition.StatusID = 9;
            _context.SchoolPositions.Update(availablePosition);

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



        public async Task<Employee> GetNonTeacherHomePageAsync(int employeeID)
        {
            Console.WriteLine($"Fetching data for EmployeeID: {employeeID}");

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
                 .Include(e => e.PersonalDetails.CasteCategory)
                 .Include(e => e.PersonalDetails.BloodGroup)
                 .Include(e => e.PersonalDetails.MaritalStatus)
                 .Include(e => e.PersonalDetails.District)
                 .FirstOrDefaultAsync(c => c.EmployeeID == employeeID);

            if (employee == null) return null;

            return employee;
        }

        public async Task<Employee?> UpdateNonTeacherAsync(int Id, Employee updatedEmployee)
        {
            // Log entering the method
            _logger.LogInformation("Entering UpdateEmployeeAsync for EmployeeID: {Id}", Id);

            // Retrieve the existing employee from the database by ID
            var existingEmployee = await _context.Employees
                .Include(e => e.PersonalDetails)
                .Include(e => e.EmployeeDocuments)
                .Include(e => e.EmployeeEducations)
                .FirstOrDefaultAsync(e => e.EmployeeID == Id)
                .ConfigureAwait(false);

            int newStatusID = await _context.Statuses
                 .Where(s => s.StatusText == "New" && s.StatusType == "Employee")
                 .Select(s => s.StatusID)
                 .FirstOrDefaultAsync();

            int pendingStatusID = await _context.Statuses
                .Where(s => s.StatusText == "Pending" && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            int statusID = existingEmployee.StatusID ?? throw new InvalidOperationException("StatusID cannot be null.");

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
                existingEmployee.PersonalDetails.ProtectedTeacher = updatedEmployee.PersonalDetails.ProtectedTeacher;
                existingEmployee.PersonalDetails.EligibilityTestQualified = existingEmployee.PersonalDetails.EligibilityTestQualified;
            }

            // Update employee educations if any
            if (updatedEmployee.EmployeeEducations != null)
            {
                // Clear existing educations and add the new ones
                existingEmployee.EmployeeEducations.Clear();
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

                _context.Users.Update(user);
            }

            // Save changes to the database
            await _context.SaveChangesAsync().ConfigureAwait(false);


            return existingEmployee;
        }
       

        public async Task<List<Employee>> GetAllRetiredNonTeacherAsync()
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
                            e.EmployeeType.Employeetype == "Non-Teaching Staff" && e.RetirementDate <= DateTime.Now) // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Employee>> GetAllNonTeacherOnLeaveAsync()
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
                            e.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Employee>> GetNonTeachersBySchoolIDAsync(int schoolID)
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
                .Include(e => e.Status)
                .Where(e => e.SchoolID == schoolID &&
                            e.Status != null &&
                            e.Status.StatusText == "Active" &&
                            e.Status.StatusType == "Employee" &&
                            e.EmployeeType != null &&
                            e.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Employee>> GetApprovedNonTeacherAsync()
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
                            e.EmployeeType.Employeetype == "Non-Teaching Staff" &&
                            e.ApprovalType != null &&
                            e.ApprovalType.Approvaltype == "Approved")
                .ToListAsync();
        }

        public async Task<List<Employee>> GetNonApprovedNonTeacherAsync()
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
                            e.EmployeeType.Employeetype == "Non-Teaching Staff" &&
                            e.ApprovalType != null &&
                            e.ApprovalType.Approvaltype == "Non-Approved")
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetDynamicListNonTeachersDataAsync(List<string>? statusTexts, int? schoolId)
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
                    .Where(e => e.EmployeeType != null && e.EmployeeType.Employeetype == "Non-Teaching Staff");

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

        public async Task<TeacherStatusCountDTO> GetTotalNonTeachersStatusCountAsync()
        {
            return new TeacherStatusCountDTO
            {
                Active = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "Active" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Non-Teaching Staff"),

                New = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "New" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Non-Teaching Staff"),

                Pending = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "Pending" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Non-Teaching Staff"),

                HMApproved = await _context.Employees.CountAsync(s =>
                    s.Status.StatusText == "HMApproved" &&
                    s.Status.StatusType == "Employee" &&
                    s.EmployeeType.Employeetype == "Non-Teaching Staff")
            };
        }


        public async Task<List<Employee>> GetOnLeaveNonTeachersBySchoolIDAsync(int schoolID)
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
                                e.EmployeeType.Employeetype == "Non-Teaching Staff")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving employees.", ex);
            }
        }
    }
}
