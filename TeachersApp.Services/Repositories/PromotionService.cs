using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.Migrations;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO.PromotionRelinquishment;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TeachersApp.Services.Repositories
{
    public class PromotionService : IPromotionService
    {
        private readonly TeachersAppDbcontext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ITeacherHistoryService _historyService;
        private readonly ILogger<PromotionService> _logger;

        public PromotionService(TeachersAppDbcontext context, IBackgroundJobClient backgroundJobClient,ILogger<PromotionService>logger,ITeacherHistoryService teacherHistoryService)
        {
            _context = context;
            _backgroundJobClient = backgroundJobClient;
            _logger = logger;
            _historyService = teacherHistoryService;
        }

        public async Task UpdatePromotionSeniorityAsync()
        {
            // Fetch all employees with their designations
            var employees = await _context.Employees
                .Include(e => e.Designation)
                .ToListAsync();

            // Reset PromotionSeniorityNumber for non-eligible employees
            foreach (var employee in employees.Where(e => e.PromotionEligible == false))
            {
                employee.PromotionSeniorityNumber = null;
            }

            // Separate LPSA and other designation employees
            var lpsaEmployees = employees
                .Where(e => e.PromotionEligible && e.Designation?.DesignationText.ToUpper() == "LPSA")
                .GroupBy(e => e.DesignationID)
                .ToList();

            var otherEmployees = employees
                .Where(e => e.PromotionEligible && e.Designation?.DesignationText.ToUpper() != "LPSA")
                .GroupBy(e => new { e.DesignationID, e.SubjectID })
                .ToList();

            // Process LPSA employees
            foreach (var group in lpsaEmployees)
            {
                var sortedGroup = group.OrderBy(e => e.HireDate).ToList();
                int seniorityNumber = 1;

                foreach (var employee in sortedGroup)
                {
                    if (employee.DesignationID != null)
                    {
                        employee.PromotionSeniorityNumber = seniorityNumber++;
                    }
                    else
                    {
                        employee.PromotionSeniorityNumber = null;
                    }
                }
            }

            // Process other designation employees
            foreach (var group in otherEmployees)
            {
                var sortedGroup = group.OrderBy(e => e.HireDate).ToList();
                int seniorityNumber = 1;

                foreach (var employee in sortedGroup)
                {
                    if (employee.DesignationID != null && employee.SubjectID != null)
                    {
                        employee.PromotionSeniorityNumber = seniorityNumber++;
                    }
                    else
                    {
                        employee.PromotionSeniorityNumber = null;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }



        public async Task<List<Employee>> GetSortedPromotionEligibleEmployeesAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.PromotionRelinquishments)
                .Include(e => e.School)
                .Include(e => e.Subject)
                .Include(e => e.Designation)
                    .ThenInclude(d => d.DesignationQualifications)
                .Include(e => e.EmployeeEducations)
                    .ThenInclude(edu => edu.Course)
                .Include(e => e.Promotions) // Ensure Promotions are included
                .Where(e => e.EmployeeEducations.Any() && e.Designation != null) // Pre-filter
                .ToListAsync();

            // Filter employees eligible for promotion and exclude those with completed promotions
            var promotionEligibleEmployees = employees
                .Where(e => e.PromotionEligible == true) 
                .OrderBy(e => e.HireDate)
                .ThenBy(e => GetQualificationPriority(e))
                .ToList();

            return promotionEligibleEmployees;
        }


        public async Task<bool> PromotionEligible(Employee employee)
        {
            var currentDesignationId = employee.DesignationID;

            if (currentDesignationId <= 0 || !currentDesignationId.HasValue)
            {
                return false;
            }

            var nextDesignationId = await GetNextDesignationId(currentDesignationId.Value);

            if (nextDesignationId == null)
            {
                return false;
            }

            var requiredQualificationIds = GetQualificationIdsForDesignation(nextDesignationId.Value);

            if (requiredQualificationIds == null || !requiredQualificationIds.Any())
            {
                return false;
            }

            var employeeQualifications = GetEmployeeUploadedQualifications(employee.EmployeeID);

            if (employeeQualifications == null || !employeeQualifications.Any())
            {
                return false;
            }

            bool isEligible = requiredQualificationIds.Any(requiredId => employeeQualifications.Contains(requiredId));

            return isEligible;
        }

        //Make employee.PromotionEligible =true

        public async Task<int?> GetNextDesignationId(int currentDesignationId)
        {
            var nextDesignation = await _context.Designations
                .Where(d => d.DesignationID > currentDesignationId)
                .OrderBy(d => d.DesignationID)
                .FirstOrDefaultAsync();

            return nextDesignation?.DesignationID;
        }

        public List<int> GetQualificationIdsForDesignation(int designationId)
        {
            var qualifications = _context.DesignationQualifications
                                         .Where(dq => dq.DesignationID == designationId)
                                         .Select(dq => dq.QualificationID)
                                         .ToList();

            return qualifications ?? new List<int>();
        }

        public List<int?> GetEmployeeUploadedQualifications(int employeeId)
        {
            var uploadedQualifications = _context.EmployeeEducations
                                                  .Where(eq => eq.EmployeeID == employeeId)
                                                  .Select(eq => eq.CourseID)
                                                  .ToList();

            return uploadedQualifications;
        }

        public int GetQualificationPriority(Employee employee)
        {
            var priorityQualifications = new HashSet<string> { "KTET I", "KTET II", "KTET III", "SET" };
            if (employee.EmployeeEducations.Any(edu => priorityQualifications.Contains(edu.Course.CourseName)))
            {
                return 1; // Highest priority
            }

            var exceptionalQualifications = new HashSet<string> { "PhD", "MEd", "NET" };
            if (employee.EmployeeEducations.Any(edu => exceptionalQualifications.Contains(edu.Course.CourseName)))
            {
                return 2; // Second-highest priority
            }

            return 3; // Lowest priority
        }




        public async Task<IEnumerable<GetPromotionListDTO>> GetEmployeesByDesignationAndSubjectAsync(int designationId, int? subjectId)
        {
            try
            {
                // First get the designation to check if it's LPSA
                var designation = await _context.Designations
                    .FirstOrDefaultAsync(d => d.DesignationID == designationId);

                if (designation == null)
                {
                    return Enumerable.Empty<GetPromotionListDTO>();
                }

                var query = _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.Subject)
                    .Include(e => e.School)
                    .Where(e => e.DesignationID == designationId
                            && e.PromotionEligible == true
                            && e.PromotionSeniorityNumber != null);

                // For LPSA, ignore subjectId in the query
                // For other designations, apply subject filter if subjectId is provided
                if (designation.DesignationText.ToUpper() != "LPSA" && subjectId.HasValue)
                {
                    query = query.Where(e => e.SubjectID == subjectId.Value);
                }

                var employees = await query
                    .OrderBy(e => e.PromotionSeniorityNumber)
                    .ToListAsync();

                // Check if no employees were found
                if (!employees.Any())
                {
                    return Enumerable.Empty<GetPromotionListDTO>();
                }

                // Map Employee entities to GetPromotionListDTO using extension method
                return employees.Select(e => e.ToGetPromotionEmployeeDTO());
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving employee promotion list.", ex);
            }
        }


        public async Task<IEnumerable<GetPromotionListDTO>> GetEmployeesByDesignationAsync(int designationId)
        {
            try
            {
                // Fetch all employees matching the designationId and subjectId
                var employees = await _context.Employees
                    .Include(e => e.Designation)
                    .Include(e => e.Subject)
                    .Include(e => e.School)
                    .Where(e => e.DesignationID == designationId
                     && e.PromotionEligible == true
                     && e.PromotionSeniorityNumber != null)
                    .OrderBy(e => e.PromotionSeniorityNumber)
                    .ToListAsync();

                // Check if no employees were found
                if (employees == null || !employees.Any())
                {
                    // Handle case where no employees are found
                    return Enumerable.Empty<GetPromotionListDTO>();
                }

                // Map Employee entities to GetPromotionListDTO using extension method
                return employees.Select(e => e.ToGetPromotionEmployeeDTO());
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving employee promotion list.", ex);
            }

        }

        public async Task<Promotion> CreatePromotionRequestAsync(Promotion promotionRequest)
        {
            // Check if the employee exists
            var employee = await _context.Employees
                .Where(e => e.EmployeeID == promotionRequest.EmployeeID)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found.");
            }

            // Set `PromotedFromDesignationID` from Employee
            promotionRequest.PromotedFromDesignationID = employee.DesignationID ?? 0;

            // Get the next designation
            var nextDesignation = await _context.Designations
                .Where(d => d.DesignationID > employee.DesignationID)
                .OrderBy(d => d.DesignationID)
                .FirstOrDefaultAsync();

            if (nextDesignation == null)
            {
                throw new InvalidOperationException("Next designation not found.");
            }

            promotionRequest.FromSchoolID = employee.SchoolID ?? throw new InvalidOperationException("SchoolID cannot be null."); ;
            promotionRequest.PromotedToDesignationID = nextDesignation.DesignationID; 
            promotionRequest.RequestDate = DateTime.UtcNow;
            promotionRequest.PromotionDate = null;
            

            // Get the pending status ID for promotion
            promotionRequest.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "Pending" && s.StatusType == "Promotion")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

           

            // Add to database and save
            await _context.Promotions.AddAsync(promotionRequest);
            await _context.SaveChangesAsync();

            return promotionRequest;
        }
       



        public async Task UpdateEmployeeDetailsOnTransferDate(int employeeId, int newSchoolId, DateTime promotionDate)
        {
            //Retrieve employee from the database

           var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
               // Update employee's school and work start date
                 employee.SchoolID = newSchoolId;    // Set to new school
                employee.WorkStartDate = promotionDate; // Set to transfer date
                await _context.SaveChangesAsync();
            }
        }

        private async Task HandleTeacherHistoryAsync(Promotion promotionRequest, Employee employee, DateTime promotionDate)
        {
            var teacherHistory = new TeacherHistory
            {
                EmployeeID = employee.EmployeeID,
                ChangeDate = promotionDate,
                ChangedByID = promotionRequest.ApprovedByID,
                ChangeDescription = "Promotion to " + (promotionRequest.PromotedToDesignation?.DesignationText ?? "Unknown"),
                ChangeTypeID = await _historyService.GetChangeTypeIdAsync("Promotion"),
                ChangeFromSchoolID = employee.SchoolID,
                ChangeToSchoolID = promotionRequest.ToSchoolIDApproved ?? employee.SchoolID,
                PromotedFromPositionID = promotionRequest.PromotedFromDesignationID,
                PromotedToPositionID = promotionRequest.PromotedToDesignationID != 0
                    ? promotionRequest.PromotedToDesignationID
                    : employee.DesignationID
            };

            // If promotionDate is today, directly update
            if (promotionDate.Date == DateTime.UtcNow.Date)
            {
                _context.TeacherHistories.Add(teacherHistory);
                await _context.SaveChangesAsync();
            }
            else if (promotionDate > DateTime.UtcNow)
            {
                // Schedule update for future promotions
                _backgroundJobClient.Schedule(() =>
                    _historyService.CreateTeacherHistoryAsync(teacherHistory),
                    promotionDate.ToUniversalTime());
            }
        }




        public async Task UpdateEmployeeDetailsOnPromotionDate(int employeeId, int newDesignationId)
        {
            //Retrieve employee from the database

           var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
               // Update employee's designation
                 employee.DesignationID = newDesignationId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateStatusToCompletedAsync(int id)
        {
            //Retrieve the promotion request
             var promotionRequest = await _context.Promotions
                 .FirstOrDefaultAsync(pr => pr.PromotionID == id);

            if (promotionRequest != null)
            {
                promotionRequest.StatusID = await _context.Statuses
                    .Where(s => s.StatusText == "Completed" && s.StatusType == "Promotion")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();

                if (promotionRequest.EmployeeID > 0 )
                {
                    // Retrieve the associated employee
                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.EmployeeID == promotionRequest.EmployeeID);

                    if (employee != null)
                    {
                        // Check promotion eligibility
                        bool isEligible = await PromotionEligible(employee);

                        // Update the PromotionEligible flag
                        employee.PromotionEligible = isEligible;
                    }
                }




                await _context.SaveChangesAsync();
            }
        }
        public async Task<Promotion?> ApprovePromotionRequestAsync(int id, ApprovePromotionRequestDTO approvePromotionRequestDTO)
        {
            var promotionRequest = await _context.Promotions
            .Include(pr => pr.Employee)
            .Include(pr => pr.PromotedToDesignation) // Include related navigation properties
            .Include(pr => pr.PromotedFromDesignation)
            .Include(pr => pr.FromSchool) // Include related navigation properties, if needed
            .Include(pr => pr.ToSchoolApproved) // Removed since it's not a navigation property
            .FirstOrDefaultAsync(pr => pr.PromotionID == id);

            if (promotionRequest == null)
                return null;

            // Update promotion request details
            promotionRequest.StatusChangeDate = DateTime.Now;
            promotionRequest.StatusID = await GetStatusIdAsync("Approved", "Promotion");
            promotionRequest.PromotionDate = approvePromotionRequestDTO.PromotionDate;
            promotionRequest.ApproverComment = approvePromotionRequestDTO.ApproverComment;
            promotionRequest.ToSchoolIDApproved = approvePromotionRequestDTO.ApprovedSchoolID;

            var employee = promotionRequest.Employee;

            if (employee != null)
            {
                if (promotionRequest.PromotionDate == null)
                {
                    throw new InvalidOperationException("PromotionDate cannot be null.");
                }

                DateTime promotionDate = promotionRequest.PromotionDate.Value;
                // Handle school transfer logic
                await HandleSchoolTransferAsync(promotionRequest, employee, promotionDate);

                // Handle Teacher History logic
                await HandleTeacherHistoryAsync(promotionRequest, employee, promotionDate);

                // Handle promotion designation logic
                await HandlePromotionDesignationAsync(promotionRequest, employee, promotionDate);

                // Schedule or update status based on promotion date
                await HandlePromotionStatusAsync(id, promotionDate);
            }

            // Save changes
            await _context.SaveChangesAsync();
            return promotionRequest;
        }

        private async Task<int> GetStatusIdAsync(string statusText, string statusType)
        {
            return await _context.Statuses
                .Where(s => s.StatusText == statusText && s.StatusType == statusType)
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();
        }

        private async Task HandleSchoolTransferAsync(Promotion promotionRequest, Employee employee, DateTime promotionDate)
        {
            if (promotionRequest.ToSchoolIDApproved.HasValue &&
                employee.SchoolID != promotionRequest.ToSchoolIDApproved.Value)
            {
                if (promotionDate.Date == DateTime.UtcNow.Date)
                {
                    // Immediate update for today's promotion
                    employee.SchoolID = promotionRequest.ToSchoolIDApproved.Value;
                    employee.WorkStartDate = promotionDate;
                }
                else if (promotionDate > DateTime.UtcNow)
                {
                    // Schedule update for future promotions
                    _backgroundJobClient.Schedule(() =>
                        UpdateEmployeeDetailsOnTransferDate(employee.EmployeeID, promotionRequest.ToSchoolIDApproved.Value, promotionDate),
                        promotionDate.ToUniversalTime());
                }
            }
        }



        private async Task HandlePromotionDesignationAsync(Promotion promotionRequest, Employee employee, DateTime promotionDate)
        {
            if (promotionRequest.PromotedToDesignationID == 0)
                throw new InvalidOperationException("PromotedToDesignationID is invalid (0).");

            if (promotionDate.Date == DateTime.UtcNow.Date)
            {
                employee.DesignationID = promotionRequest.PromotedToDesignationID;
            }
            else if (promotionDate > DateTime.UtcNow)
            {
                _backgroundJobClient.Schedule(() =>
                    UpdateEmployeeDetailsOnPromotionDate(employee.EmployeeID, promotionRequest.PromotedToDesignationID),
                    promotionDate.ToUniversalTime());
            }
        }

        private async Task HandlePromotionStatusAsync(int promotionId, DateTime promotionDate)
        {
            if (promotionDate > DateTime.UtcNow)
            {
                _backgroundJobClient.Schedule(
                    () => UpdateStatusToCompletedAsync(promotionId),
                    promotionDate.ToUniversalTime());
            }
            else
            {
                await UpdateStatusToCompletedAsync(promotionId);
            }
        }


        public async Task<Promotion?> RejectPromotionRequestAsync(int id, Promotion PromotionReject)
        {
            var promotionRequest = await _context.Promotions
                .Include(tr => tr.Employee)
                .FirstOrDefaultAsync(tr => tr.PromotionID == id);

            if (promotionRequest == null)
            {
                return null; // or throw an exception if preferred
            }
            // Retrieve the "Approved" status ID
            var approvedStatusId = await _context.Statuses
                .Where(s => s.StatusText == "Approved" && s.StatusType == "Promotion")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            // Check if the current status is "Approved"
            if (promotionRequest.StatusID == approvedStatusId)
            {
                throw new InvalidOperationException("Promotion request cannot be rejected after being approved.");
            }

            // Retrieve the "Rejected" status ID
            var rejectedStatusId = await _context.Statuses
                .Where(s => s.StatusText == "Rejected" && s.StatusType == "Promotion")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();
            

            // Update status to "Rejected"
            promotionRequest.StatusID = rejectedStatusId;
            promotionRequest.PromotionDate = null;
            promotionRequest.StatusChangeDate = DateTime.Now;

            // Update properties

            promotionRequest.ApproverComment = PromotionReject.ApproverComment;

            // Save changes to the database
            _context.Promotions.Update(promotionRequest);
            await _context.SaveChangesAsync();

            return promotionRequest;
        }


        public async Task<List<Promotion>> FilterTeacherPromotionCompletedListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null,
            DateTime? fromPromotionDate = null,
            DateTime? toPromotionDate = null)
        {
            var query = _context.Promotions.AsQueryable();

            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.ToSchoolIDApproved == schoolID.Value || p.FromSchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.PromotedToDesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromPromotionDate.HasValue)
            {
                query = query.Where(p => p.PromotionDate >= fromPromotionDate.Value);
            }

            if (toPromotionDate.HasValue)
            {
                query = query.Where(p => p.PromotionDate <= toPromotionDate.Value);
            }

            // Apply additional conditions for status and employee type
            query = query.Where(e =>
                e.Status != null &&
                e.Status.StatusText == "Completed" &&
                e.Status.StatusType == "Promotion" &&
             e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                e.Employee.EmployeeType != null &&
                e.Employee.EmployeeType.Employeetype == "Teaching Staff");

            // Retrieve the filtered or unfiltered data with necessary includes
            return await query
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.EmployeeType)
                .Include(p => p.FromSchool)
                .Include(p => p.ToSchoolApproved)
                .Include(p => p.PromotedFromDesignation)
                .Include(p => p.PromotedToDesignation)
                .Include(p => p.RequestedByUser)
                .Include(p => p.ApprovedByUser)
                .Include(p => p.Status)
                .ToListAsync();
        }

        public async Task<List<Promotion>> FilterNonTeacherPromotionCompletedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromPromotionDate = null,
           DateTime? toPromotionDate = null)
        {
            var query = _context.Promotions.AsQueryable();

            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.ToSchoolIDApproved == schoolID.Value || p.FromSchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.PromotedToDesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromPromotionDate.HasValue)
            {
                query = query.Where(p => p.PromotionDate >= fromPromotionDate.Value);
            }

            if (toPromotionDate.HasValue)
            {
                query = query.Where(p => p.PromotionDate <= toPromotionDate.Value);
            }

            // Apply additional conditions for status and employee type
            query = query.Where(e =>
                e.Status != null &&
                e.Status.StatusText == "Completed" &&
                e.Status.StatusType == "Promotion" &&
              e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                e.Employee.EmployeeType != null &&
                e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff");

            // Retrieve the filtered or unfiltered data with necessary includes
            return await query
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.EmployeeType)
                .Include(p => p.FromSchool)
                .Include(p => p.ToSchoolApproved)
                .Include(p => p.PromotedFromDesignation)
                .Include(p => p.PromotedToDesignation)
                .Include(p => p.RequestedByUser)
                .Include(p => p.ApprovedByUser)
                .Include(p => p.Status)
                .ToListAsync();
        }

        public async Task<List<Promotion>> FilterTeacherPromotionRequestListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null,
            DateTime? fromPromotionDate = null,
            DateTime? toPromotionDate = null)
        {
            var query = _context.Promotions.AsQueryable();

            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.ToSchoolIDApproved == schoolID.Value || p.FromSchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.PromotedFromDesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromPromotionDate.HasValue)
            {
                query = query.Where(p => p.RequestDate >= fromPromotionDate.Value);
            }

            if (toPromotionDate.HasValue)
            {
                query = query.Where(p => p.RequestDate <= toPromotionDate.Value);
            }

            // Apply additional conditions for status and employee type
            query = query.Where(e =>
                e.Status != null &&
                e.Status.StatusText != "Completed" &&
                e.Status.StatusType == "Promotion" &&
               e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                e.Employee.EmployeeType != null &&
                e.Employee.EmployeeType.Employeetype == "Teaching Staff");

            // Retrieve the filtered or unfiltered data with necessary includes
            return await query
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.EmployeeType)
                .Include(p => p.FromSchool)
                .Include(p => p.ToSchoolApproved)
                .Include(p => p.PromotedFromDesignation)
                .Include(p => p.PromotedToDesignation)
                .Include(p => p.RequestedByUser)
                .Include(p => p.ApprovedByUser)
                .Include(p => p.Status)
                .ToListAsync();
        }


        public async Task<List<Promotion>> FilterNonTeacherPromotionRequestListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null,
            DateTime? fromPromotionDate = null,
            DateTime? toPromotionDate = null)
        {
            var query = _context.Promotions.AsQueryable();

            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.ToSchoolIDApproved == schoolID.Value || p.FromSchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.PromotedFromDesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromPromotionDate.HasValue)
            {
                query = query.Where(p => p.RequestDate >= fromPromotionDate.Value);
            }

            if (toPromotionDate.HasValue)
            {
                query = query.Where(p => p.RequestDate <= toPromotionDate.Value);
            }

            // Apply additional conditions for status and employee type
            query = query.Where(e =>
                e.Status != null &&
                e.Status.StatusText != "Completed" &&
                e.Status.StatusType == "Promotion" &&
             e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                e.Employee.EmployeeType != null &&
                e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff");

            // Retrieve the filtered or unfiltered data with necessary includes
            return await query
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.EmployeeType)
                .Include(p => p.FromSchool)
                .Include(p => p.ToSchoolApproved)
                .Include(p => p.PromotedFromDesignation)
                .Include(p => p.PromotedToDesignation)
                .Include(p => p.RequestedByUser)
                .Include(p => p.ApprovedByUser)
                .Include(p => p.Status)
                .ToListAsync();
        }
        public async Task<List<Promotion>> GetAllNonTeacherPromotionRequestAsync()
        {
            return await _context.Promotions
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.PromotedFromDesignation)
                .Include(tr => tr.PromotedToDesignation)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                 .Where(e => e.Status != null && e.Status.StatusType == "Promotion" &&
                            e.Status.StatusText != "Completed" &&
                            e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Promotion>> GetAllTeacherPromotionRequestAsync()
        {
            return await _context.Promotions
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.PromotedFromDesignation)
                .Include(tr => tr.PromotedToDesignation)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                 .Where(e => e.Status != null && e.Status.StatusType == "Promotion" &&
                            e.Status.StatusText != "Completed" &&
                         e.Employee.Status.StatusType == "Employee" &&
                         (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Promotion>> GetAllPromotedNonTeacherAsync()
        {
            return await _context.Promotions
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.PromotedFromDesignation)
                .Include(tr => tr.PromotedToDesignation)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Status != null && e.Status.StatusType == "Promotion" &&
                            e.Status.StatusText == "Completed" &&
                          e.Employee.Status.StatusType == "Employee" &&
                         (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<Promotion>> GetAllPromotedTeacherAsync()
        {
            return await _context.Promotions
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.PromotedFromDesignation)
                .Include(tr => tr.PromotedToDesignation)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Status != null && e.Status.StatusType == "Promotion" &&
                            e.Status.StatusText == "Completed" &&
                           e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<PromotionRelinquishment> CreatePromotionRelinquishment(PromotionRelinquishment promotionRelinquishment)
        {
            _context.PromotionRelinquishments.Add(promotionRelinquishment);

            //Make Approval status make false
            promotionRelinquishment.ApprovalStatus = false;
            await _context.SaveChangesAsync();
            return promotionRelinquishment;
        }




        public async Task<List<PromotionRelinquishment>> GetAllPromotionRelinquishmentsAsync()
        {
            var relinquishments = await _context.PromotionRelinquishments
                .Include(r => r.Employee)
                .ThenInclude(e => e.Designation) // Include current designation
                .Include(r => r.Document)
                .ToListAsync();

            return relinquishments;
        }


        public async Task<PromotionRelinquishment?> ApprovePromotionRelinquishmentAsync(int id, ApprovePromotionRelinquishmentDTO updateDTO)
        {
            var relinquishment = await _context.PromotionRelinquishments
               .Include(s => s.Document)
               .Include(s => s.Employee)
               .FirstOrDefaultAsync(s => s.RelinquishmentID == id);

            if (relinquishment == null)
                return null;

            relinquishment.ApprovalStatus = updateDTO.ApprovalStatus;


            _context.Update(relinquishment);
            await _context.SaveChangesAsync();

            return relinquishment;
        }

        public async Task<List<Promotion>> GetPromotionRequestByEmployeeIdAsync(int employeeID)
        {
            var employeeExists = await _context.Promotions.AnyAsync(s => s.EmployeeID == employeeID);
          
            return await _context.Promotions
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.PromotedFromDesignation)
                .Include(tr => tr.PromotedToDesignation)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                 .Where(tr => tr.EmployeeID == employeeID)// Filter for Teaching Staff
                 .ToListAsync();


        }
        public async Task<List<Promotion>> GetTeachersPromotionRequestsBySchoolIDAsync(int schoolID)
        {
            bool schoolExists = await _context.Employees.AnyAsync(s => s.SchoolID == schoolID);
            if (!schoolExists)
            {
                return null;
            }
            var Exists = await _context.Promotions.AnyAsync(s => s.Employee.SchoolID == schoolID);

            return await _context.Promotions
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.PromotedFromDesignation)
                .Include(tr => tr.PromotedToDesignation)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Employee.SchoolID == schoolID &&
                 e.Employee.EmployeeType.Employeetype != null &&
                 e.Employee.EmployeeType.Employeetype == "Teaching Staff"
                ) // Filter for Teaching Staff
                .ToListAsync();
        }
    }
}
