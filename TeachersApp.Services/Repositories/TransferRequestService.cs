using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.Migrations;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Services.Repositories
{
    public class TransferRequestService : ITransferRequestService
    {
        private readonly TeachersAppDbcontext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ITeacherHistoryService _historyService;

        public TransferRequestService(TeachersAppDbcontext context, IBackgroundJobClient backgroundJobClient, ITeacherHistoryService historyService)
        {
            _context = context;
            _backgroundJobClient = backgroundJobClient;
            _historyService = historyService;
        }

        public async Task<TransferRequest> CreateTransferRequestAsync(TransferRequest transferRequest)
        {

            // Get the SchoolID for the specified EmployeeID in the transfer request
            var employee = await _context.Employees
          .Where(e => e.EmployeeID == transferRequest.EmployeeID)
          .FirstOrDefaultAsync();

            if (employee == null)
            {
                // Handle case where the employee doesn't exist
                throw new InvalidOperationException("Employee not found.");
            }

            transferRequest.FromSchoolID = employee.SchoolID ?? 0; // If SchoolID is null, set FromSchoolID to 0
            transferRequest.RequestDate = DateTime.Now;
            transferRequest.StatusID = await _context.Statuses
               .Where(s => s.StatusText == "Pending" && s.StatusType == "TransferRequest")
               .Select(s => s.StatusID)
               .FirstOrDefaultAsync();


            await _context.TransferRequests.AddAsync(transferRequest);
            await _context.SaveChangesAsync();

            return transferRequest;
        }

        public async Task<List<TransferRequest>> GetAllNonTeacherTransferRequestAsync()
        {
            return await _context.TransferRequests
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                 .Where(e => e.Status != null && e.Status.StatusType == "TransferRequest" &&
                            e.Status.StatusText != "Completed" &&
                           e.Employee.Status.StatusType == "Employee" &&
                            (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }
        public async Task<List<TransferRequest>> GetAllTeacherTransferRequestAsync()
        {
            return await _context.TransferRequests
                .Include(tr => tr.Employee)
                    .ThenInclude(tr => tr.EmployeeType)
                    .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                 .Where(e => e.Status != null && e.Status.StatusType == "TransferRequest" &&
                            e.Status.StatusText != "Completed" &&
                            e.Employee.Status.StatusType == "Employee" &&
                            (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();


        }

        public async Task<List<TransferRequest>> GetAllTransferedNonTeacherAsync()
        {
            return await _context.TransferRequests
                .Include(tr => tr.Employee)
                    .ThenInclude(tr => tr.EmployeeType)
                    .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
               .Where(e => e.Status != null && e.Status.StatusType == "TransferRequest" &&
                            e.Status.StatusText == "Completed" &&
                            e.Employee.Status.StatusType == "Employee" &&
                            (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<TransferRequest>> GetAllTransferedTeacherAsync()
        {
            return await _context.TransferRequests
               .Include(tr => tr.Employee)
                   .ThenInclude(tr => tr.EmployeeType)
                   .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
               .Include(tr => tr.FromSchool)
               .Include(tr => tr.ToSchool_One)
               .Include(tr => tr.ToSchool_Two)
               .Include(tr => tr.ToSchool_Three)
               .Include(tr => tr.ToSchoolApproved)
               .Include(tr => tr.RequestedByUser)
               .Include(tr => tr.ApprovedByUser)
               .Include(tr => tr.Status)
               .Where(e => e.Status != null && e.Status.StatusType == "TransferRequest" &&
                            e.Status.StatusText == "Completed" &&
                            e.Employee.Status.StatusType == "Employee" &&
                            (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }



        public async Task<TransferRequest?> ApproveTransferAsync(int id, ApproveTransferRequestDTO approveTransferRequestDto)
        {
            // Retrieve the transfer request with related entities
            var transferRequest = await _context.TransferRequests
                .Include(tr => tr.Employee)
                .Include(tr => tr.ToSchoolApproved)
                .FirstOrDefaultAsync(tr => tr.TransferRequestID == id);

            if (transferRequest == null)
                return null;

            transferRequest.StatusChangeDate = DateTime.Now;

            // Set status to "Approved"
            transferRequest.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "Approved" && s.StatusType == "TransferRequest")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            // Apply changes from the DTO
            transferRequest.ToSchoolIDApproved = approveTransferRequestDto.ToSchoolIDApproved;
            transferRequest.TransferDate = approveTransferRequestDto.TransferDate;
            transferRequest.ApproverComment = approveTransferRequestDto.ApproverComment;
            transferRequest.FilePath = approveTransferRequestDto.FilePath;

            var employee = transferRequest.Employee;
            if (employee != null && approveTransferRequestDto.TransferDate.HasValue)
            {
                DateTime transferDate = approveTransferRequestDto.TransferDate.Value;

                if (transferDate.Date == DateTime.Now.Date) // If transfer date is today
                {
                    if (transferRequest.ToSchoolIDApproved.HasValue)
                    {
                        employee.SchoolID = transferRequest.ToSchoolIDApproved.Value; // Update employee's school
                        employee.WorkStartDate = transferDate; // Set transfer date
                    }
                    else
                    {
                        throw new InvalidOperationException("ToSchoolIDApproved is null."); // Handle invalid data
                    }
                }
                else if (transferDate > DateTime.UtcNow) // If transfer date is in the future
                {
                    if (transferRequest.ToSchoolIDApproved.HasValue)
                    {
                        // Schedule Hangfire jobs for future transfer
                        await ScheduleFutureTransfer(
                            transferRequest.TransferRequestID,
                            employee.EmployeeID,
                            transferRequest.ToSchoolIDApproved.Value,
                            transferDate
                        );
                    }
                    else
                    {
                        throw new InvalidOperationException("ToSchoolIDApproved is null."); // Handle invalid data
                    }
                }
            }

            // Save initial changes
            await _context.SaveChangesAsync();

            // Additional logic for immediate transfers
            if (approveTransferRequestDto.TransferDate.HasValue && approveTransferRequestDto.TransferDate.Value.Date == DateTime.Now.Date)
            {
                // Update status and create teacher history for today
                await UpdateStatusToCompletedAsync(transferRequest.TransferRequestID);
                await CreateTeacherHistoryAsync(transferRequest.TransferRequestID, employee.EmployeeID, approveTransferRequestDto.TransferDate.Value);
            }

            return transferRequest;
        }

        public async Task ScheduleFutureTransfer(int transferRequestId, int employeeId, int newSchoolId, DateTime transferDate)
        {
            try
            {
                // Schedule the status update
                _backgroundJobClient.Schedule(() =>
                    UpdateStatusToCompletedAsync(transferRequestId),
                    transferDate.ToUniversalTime());

                // Schedule the employee details update
                _backgroundJobClient.Schedule(() =>
                    UpdateEmployeeDetailsOnTransferDateAsync(employeeId, newSchoolId, transferDate),
                    transferDate.ToUniversalTime());

                // Schedule the teacher history update
                _backgroundJobClient.Schedule(() =>
                    CreateTeacherHistoryAsync(transferRequestId, employeeId, transferDate),
                    transferDate.ToUniversalTime());
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception
                Console.WriteLine($"Error scheduling Hangfire jobs: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateEmployeeDetailsOnTransferDateAsync(int employeeId, int newSchoolId, DateTime transferDate)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                employee.SchoolID = newSchoolId;
                employee.WorkStartDate = transferDate;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateStatusToCompletedAsync(int transferRequestId)
        {
            var transferRequest = await _context.TransferRequests
                .FirstOrDefaultAsync(tr => tr.TransferRequestID == transferRequestId);

            if (transferRequest != null)
            {
                transferRequest.StatusID = await _context.Statuses
                    .Where(s => s.StatusText == "Completed" && s.StatusType == "TransferRequest")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();

                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateTeacherHistoryAsync(int transferRequestId, int employeeId, DateTime transferDate)
        {
            var transferRequest = await _context.TransferRequests
                .Include(tr => tr.ToSchoolApproved)
                .FirstOrDefaultAsync(tr => tr.TransferRequestID == transferRequestId);

            var employee = await _context.Employees.FindAsync(employeeId);

            if (transferRequest != null && employee != null)
            {
                var teacherHistory = new TeacherHistory
                {
                    EmployeeID = employee.EmployeeID,
                    ChangeDate = transferDate,
                    ChangedByID = transferRequest.ApprovedByID,
                    ChangeDescription = "Transfer to " + (transferRequest.ToSchoolApproved?.SchoolName ?? "Unknown"),
                    ChangeTypeID = await GetChangeTypeIdAsync("Transfer Request"),
                    ChangeFromSchoolID = transferRequest.FromSchoolID,
                    ChangeToSchoolID = transferRequest.ToSchoolIDApproved,
                    PromotedFromPositionID = employee.DesignationID,
                    PromotedToPositionID = employee.DesignationID
                };

                _context.TeacherHistories.Add(teacherHistory);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<int> GetChangeTypeIdAsync(string changeTypeDescription)
        {
            var changeType = await _context.ChangeTypes
                .FirstOrDefaultAsync(ct => ct.ChangeText == changeTypeDescription);

            if (changeType == null)
                throw new InvalidOperationException($"ChangeType '{changeTypeDescription}' not found.");

            return changeType.ChangeTypeID;
        }


        public async Task<TransferRequest?> RejectTransferAsync(int id, TransferRequest transferRequestReject)
        {
            var transfer = await _context.TransferRequests
                .Include(tr => tr.Employee)
                .FirstOrDefaultAsync(tr => tr.TransferRequestID == id);

            if (transfer == null)
            {
                return null; // or throw an exception if preferred
            }
            // Retrieve the "Approved" and "Completed" status IDs
            var approveOrCompleteStatusIds = await _context.Statuses
                .Where(s => (s.StatusText == "Approved" || s.StatusText == "Completed") && s.StatusType == "TransferRequest")
                .Select(s => s.StatusID)
                .ToListAsync();

            // Check if the current status is "Approved" or "Completed"
            if (approveOrCompleteStatusIds.Contains(transfer.StatusID))
            {
                throw new InvalidOperationException("Transfer request cannot be rejected after being approved or completed.");
            }

            // Retrieve the "Rejected" status ID
            var rejectedStatusId = await _context.Statuses
                .Where(s => s.StatusText == "Rejected" && s.StatusType == "TransferRequest")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            // Update status to "Rejected"
            transfer.StatusID = rejectedStatusId;

            // Update properties

            transfer.StatusChangeDate = DateTime.Now;
            transfer.ApproverComment = transferRequestReject.ApproverComment;
          
            // Save changes to the database
            _context.TransferRequests.Update(transfer);
            await _context.SaveChangesAsync();

            return transfer;
        }

        public async Task<List<TransferRequest>> FilterTeacherTransferCompletedListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null,
            DateTime? fromTransferDate = null,
            DateTime? toTransferDate = null,
            DateTime? fromWithEffectDate = null,
            DateTime? toWithEffectDate = null)
        {
            var query = _context.TransferRequests.AsQueryable();




            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.ToSchoolIDApproved == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate >= fromTransferDate.Value);
            }

            if (toTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate <= toTransferDate.Value);
            }
            if (fromWithEffectDate.HasValue)
            {
                query = query.Where(p => p.TransferDate >= fromWithEffectDate.Value);
            }

            if (toWithEffectDate.HasValue)
            {
                query = query.Where(p => p.TransferDate <= toWithEffectDate.Value);
            }

            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText == "Completed" &&
               e.Status.StatusType == "TransferRequest" &&
               e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(p => p.Employee)
                .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                .ThenInclude(e => e.School)
                 .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }

        public async Task<List<TransferRequest>> FilterNonTeacherTransferCompletedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromTransferDate = null,
           DateTime? toTransferDate = null,
           DateTime? fromWithEffectDate = null,
           DateTime? toWithEffectDate = null)
        {
            var query = _context.TransferRequests.AsQueryable();




            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.ToSchoolIDApproved == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate >= fromTransferDate.Value);
            }

            if (toTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate <= toTransferDate.Value);
            }
            if (fromWithEffectDate.HasValue)
            {
                query = query.Where(p => p.TransferDate >= fromWithEffectDate.Value);
            }

            if (toWithEffectDate.HasValue)
            {
                query = query.Where(p => p.TransferDate <= toWithEffectDate.Value);
            }

            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText == "Completed" &&
               e.Status.StatusType == "TransferRequest" &&
             e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(p => p.Employee)
                .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                .ThenInclude(e => e.School)
                 .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }

        public async Task<List<TransferRequest>> FilterTeacherTransferRequestListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromTransferDate = null,
           DateTime? toTransferDate = null)
        {
            var query = _context.TransferRequests.AsQueryable();




            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.FromSchoolID == schoolID.Value) ;
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate >= fromTransferDate.Value);
            }

            if (toTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate <= toTransferDate.Value);
            }
         

            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText != "Completed" &&
               e.Status.StatusType == "TransferRequest" &&
              e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(p => p.Employee)
                .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                .ThenInclude(e => e.School)
                 .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }



        public async Task<List<TransferRequest>> FilterNonTeacherTransferRequestListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromTransferDate = null,
           DateTime? toTransferDate = null)
        {
            var query = _context.TransferRequests.AsQueryable();




            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.FromSchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }

            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }

            if (fromTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate >= fromTransferDate.Value);
            }

            if (toTransferDate.HasValue)
            {
                query = query.Where(p => p.RequestDate <= toTransferDate.Value);
            }


            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText != "Completed" &&
               e.Status.StatusType == "TransferRequest" &&
             e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(p => p.Employee)
                .ThenInclude(e => e.Designation)
                .Include(p => p.Employee)
                .ThenInclude(e => e.School)
                 .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }
        public async Task<List<TransferRequest>> GetTransferRequestByEmployeeIdAsync(int employeeID)
        {
            var employeeExists = await _context.TransferRequests.AnyAsync(s => s.EmployeeID == employeeID);
           
            return await _context.TransferRequests
                 .Include(tr => tr.Employee)
                     .ThenInclude(tr => tr.EmployeeType)
                     .Include(tr => tr.Employee)
                     .ThenInclude(tr => tr.Designation)
                 .Include(tr => tr.FromSchool)
                 .Include(tr => tr.ToSchool_One)
                 .Include(tr => tr.ToSchool_Two)
                 .Include(tr => tr.ToSchool_Three)
                 .Include(tr => tr.ToSchoolApproved)
                 .Include(tr => tr.RequestedByUser)
                 .Include(tr => tr.ApprovedByUser)
                 .Include(tr => tr.Status)
                 .Where(tr => tr.EmployeeID == employeeID)// Filter for Teaching Staff
                 .ToListAsync();


        }
        public async Task<List<TransferRequest>> GetTeachersTransferRequestsBySchoolIDAsync(int schoolID)
        {
            bool schoolExists = await _context.Employees.AnyAsync(s => s.SchoolID == schoolID);
            if (!schoolExists)
            {
                return null;
            }
            var Exists = await _context.TransferRequests.AnyAsync(s => s.Employee.SchoolID == schoolID);

            return await _context.TransferRequests
                .Include(tr => tr.Employee)
                    .ThenInclude(tr => tr.EmployeeType)
                 .Include(tr => tr.Employee)
                    .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.FromSchool)
                .Include(tr => tr.ToSchool_One)
                .Include(tr => tr.ToSchool_Two)
                .Include(tr => tr.ToSchool_Three)
                .Include(tr => tr.ToSchoolApproved)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Employee.SchoolID == schoolID &&
                /* e.Status != null &&
                 e.Employee.Status.StatusType == "Employee" &&
                 e.Employee.Status.StatusText == "Active" &&*/
                 e.Employee.EmployeeType.Employeetype != null &&
                 e.Employee.EmployeeType.Employeetype == "Teaching Staff"
                ) // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<TransferRequest>> GetNonTeachersTransferRequestsBySchoolIDAsync(int schoolID)
        {
            bool schoolExists = await _context.Employees.AnyAsync(s => s.SchoolID == schoolID);
            if (!schoolExists)
            {
                return null;
            }
            var Exists = await _context.TransferRequests.AnyAsync(s => s.Employee.SchoolID == schoolID);

            return await _context.TransferRequests
                 .Include(tr => tr.Employee)
                    .ThenInclude(tr => tr.EmployeeType)
                 .Include(tr => tr.Employee)
                     .ThenInclude(tr => tr.Designation)
                 .Include(tr => tr.FromSchool)
                 .Include(tr => tr.ToSchool_One)
                 .Include(tr => tr.ToSchool_Two)
                 .Include(tr => tr.ToSchool_Three)
                 .Include(tr => tr.ToSchoolApproved)
                 .Include(tr => tr.RequestedByUser)
                 .Include(tr => tr.ApprovedByUser)
                 .Include(tr => tr.Status)
                 .Where(e => e.Employee.SchoolID == schoolID &&
                 /* e.Status != null &&
                  e.Employee.Status.StatusType == "Employee" &&
                  e.Employee.Status.StatusText == "Active" &&*/
                 e.Employee.EmployeeType.Employeetype != null &&
                 e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff"
                 ) // Filter for Teaching Staff
                .ToListAsync();
        }
    }
}
