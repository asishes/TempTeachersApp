using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Services.Repositories
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly TeachersAppDbcontext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public LeaveRequestService( TeachersAppDbcontext context,IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _backgroundJobClient = backgroundJobClient;
        }
        public async Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {

            // Get the employee details and ensure the employee is active
            var employee = await _context.Employees
                .Where(e => e.EmployeeID == leaveRequest.EmployeeID && e.StatusID ==
                    _context.Statuses.Where(s => s.StatusText == "Active" && s.StatusType == "Employee")
                    .Select(s => s.StatusID)
                    .FirstOrDefault())
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                // Handle case where the employee doesn't exist or is not active
                throw new InvalidOperationException("Employee not found or not active.");
            }


            leaveRequest.RequestDate = DateTime.Now;
            leaveRequest.StatusID = await _context.Statuses
               .Where(s => s.StatusText == "Pending" && s.StatusType == "LeaveRequest")
               .Select(s => s.StatusID)
               .FirstOrDefaultAsync();


            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();

            return leaveRequest ;
        }


        public async Task<List<LeaveRequest>> GetAllNonTeacherLeaveRequestAsync()
        {
            return await _context.LeaveRequests
                .Include(tr =>  tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Status != null &&
                           e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAllTeacherLeaveRequestAsync()
        {
            return await _context.LeaveRequests
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Status != null &&
                           e.Employee.Status.StatusType == "Employee" &&
                          (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAllLeaveNonTeacherAsync()
        {
            return await _context.LeaveRequests
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Status != null && e.Status.StatusType == "LeaveRequest" &&
                            e.Status.StatusText == "Completed" &&
                           e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAllLeaveTeacherAsync()
        {
            return await _context.LeaveRequests
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.Status != null && e.Status.StatusType == "LeaveRequest" &&
                            e.Status.StatusText == "Completed" &&
                           e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
                            e.Employee.EmployeeType.Employeetype != null &&
                            e.Employee.EmployeeType.Employeetype == "Teaching Staff") // Filter for Teaching Staff
                .ToListAsync();
        }


        public async Task<LeaveRequest?> ApproveLeaveRequestByHeadMasterAsync(int Id)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(e => e.LeaveRequestID == Id);

            if (leaveRequest == null)
            {
                return null;
            }

            var statusPending = await _context.Statuses
                .FirstOrDefaultAsync(s => s.StatusText == "Pending" && s.StatusType == "LeaveRequest");

            if (statusPending == null || leaveRequest.StatusID != statusPending.StatusID)
            {
                throw new Exception("Leave request is not in Pending status.");
            }
            var status = await _context.Statuses
                .FirstOrDefaultAsync(s => s.StatusText == "HMApproved" && s.StatusType == "LeaveRequest");

            if (status == null)
            {
                throw new Exception("HMApproved status not found.");
            }

            leaveRequest.StatusID = status.StatusID;
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();

            return leaveRequest; 
        }

        // Approve leave request
        public async Task<LeaveRequest?> ApproveLeaveRequestAsync(int id, ApproveLeaveRequestDTO approveLeaveRequestDto)
        {
            // Retrieve the leave request
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .FirstOrDefaultAsync(lr => lr.LeaveRequestID == id);

            if (leaveRequest == null)
                return null;

            // Update leave request status to "Approved"
            leaveRequest.StatusChangeDate = DateTime.Now;
            leaveRequest.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "Approved" && s.StatusType == "LeaveRequest")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            leaveRequest.ApproverComment = approveLeaveRequestDto.ApproverComment;

            // Save changes
            await _context.SaveChangesAsync();

            // Schedule background jobs for status updates
            await HandleEmployeeStatusToLeaveAsync(leaveRequest.EmployeeID, leaveRequest.FromDate);
            await HandleEmployeeStatusToActiveAsync(leaveRequest.EmployeeID, leaveRequest.ToDate);

            return leaveRequest;
        }

        // Update employee status to "Leave"
        public async Task UpdateEmployeeStatusToLeave(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                var leaveStatusId = await _context.Statuses
                    .Where(s => s.StatusText == "Leave" && s.StatusType == "Employee")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();

                if (leaveStatusId != 0)
                {
                    employee.StatusID = leaveStatusId;
                    await _context.SaveChangesAsync();
                }
            }
        }

        // Update employee status to "Active" (on the day after leave ends)
        public async Task UpdateEmployeeStatusToActive(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                var activeStatusId = await _context.Statuses
                    .Where(s => s.StatusText == "Active" && s.StatusType == "Employee")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();

                if (activeStatusId != 0)
                {
                    employee.StatusID = activeStatusId;
                    await _context.SaveChangesAsync();
                }
            }
        }

        // Schedule job for leave start status (update to "Leave")
        private async Task HandleEmployeeStatusToLeaveAsync(int employeeId, DateTime leaveFromDate)
        {

                _backgroundJobClient.Schedule(
                    () => UpdateEmployeeStatusToLeave(employeeId),
                    leaveFromDate.ToUniversalTime());
            
        }

        // Schedule job for leave end status (update to "Active" the day after leave ends)
        private async Task HandleEmployeeStatusToActiveAsync(int employeeId, DateTime leaveToDate)
        {
                var activeUpdateDate = leaveToDate.AddDays(1).ToUniversalTime();

                _backgroundJobClient.Schedule(
                    () => UpdateEmployeeStatusToActive(employeeId),
                    activeUpdateDate);
            
        }



        public async Task<LeaveRequest?> RejectLeaveRequestAsync(int id, LeaveRequest leaveRequestReject)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(tr => tr.Employee)
                .FirstOrDefaultAsync(tr => tr.LeaveRequestID == id);

            if (leaveRequest == null)
            {
                return null; // or throw an exception if preferred
            }
            // Retrieve the "Approved" status ID
            var approvedStatusId = await _context.Statuses
                .Where(s => s.StatusText == "Approved" && s.StatusType == "LeaveRequest")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            // Check if the current status is "Approved"
            if (leaveRequest.StatusID == approvedStatusId)
            {
                throw new InvalidOperationException("Leave request cannot be rejected after being approved.");
            }

            // Retrieve the "Rejected" status ID
            var rejectedStatusId = await _context.Statuses
                .Where(s => s.StatusText == "Rejected" && s.StatusType == "LeaveRequest")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            // Update status to "Rejected"
            leaveRequest.StatusID = rejectedStatusId;

            // Update properties
            leaveRequest.StatusChangeDate = DateTime.Now;
            leaveRequest.ApproverComment = leaveRequestReject.ApproverComment;

            // Save changes to the database
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();

            return leaveRequest;
        }

        public async Task<List<LeaveRequest>> FilterTeacherLeaveCompletedListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null
            )
        {
            var query = _context.LeaveRequests.AsQueryable();


            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.Employee.SchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }


            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }
            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText == "Approved" &&
               e.Status.StatusType == "LeaveRequest" &&
              e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.School)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }


        public async Task<List<LeaveRequest>> FilterNonTeacherLeaveCompletedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null
           )
        {
            var query = _context.LeaveRequests.AsQueryable();


            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.Employee.SchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }


            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }
            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText == "Approved" &&
               e.Status.StatusType == "LeaveRequest" &&
               e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.School)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> FilterTeacherLeaveRequestedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null
           )
        {
            var query = _context.LeaveRequests.AsQueryable();


            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.Employee.SchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }


            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }
            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText != "Approved" &&
               e.Status.StatusType == "LeaveRequest" &&
             e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.School)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }


        public async Task<List<LeaveRequest>> FilterNonTeacherLeaveRequestedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null
           )
        {
            var query = _context.LeaveRequests.AsQueryable();


            // Apply filters only if parameters are not null
            if (schoolID.HasValue)
            {
                query = query.Where(p => p.Employee.SchoolID == schoolID.Value);
            }

            if (designationID.HasValue)
            {
                query = query.Where(p => p.Employee.DesignationID == designationID.Value);
            }


            if (!string.IsNullOrEmpty(uniqueID))
            {
                query = query.Where(p => p.Employee.UniqueID == uniqueID);
            }
            query = query.Where(e =>
               e.Status != null &&
               e.Status.StatusText != "Approved" &&
               e.Status.StatusType == "LeaveRequest" &&
             e.Employee.Status.StatusType == "Employee" &&
                    (e.Employee.Status.StatusText == "Active" || e.Employee.Status.StatusText == "Leave") &&
               e.Employee.EmployeeType != null &&
               e.Employee.EmployeeType.Employeetype == "Non-Teaching Staff");

            // Retrieve the filtered or unfiltered data
            return await query
                .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.Designation)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.School)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeID)
        {
            var employeeExists = await _context.LeaveRequests.AnyAsync(s => s.EmployeeID == employeeID);

            return await _context.LeaveRequests
               .Include(tr => tr.Document)
                .Include(tr => tr.Employee)
                .ThenInclude(tr => tr.EmployeeType)
                .Include(tr => tr.RequestedByUser)
                .Include(tr => tr.ApprovedByUser)
                .Include(tr => tr.Status)
                .Where(e => e.EmployeeID == employeeID)       
                .ToListAsync();


        }
        public async Task<List<LeaveRequest>> GetTeachersLeaveRequestsBySchoolIDAsync(int schoolID)
        {
            bool schoolExists = await _context.Employees.AnyAsync(s => s.SchoolID == schoolID);
            if (!schoolExists)
            {
                return null;
            }
            var Exists = await _context.LeaveRequests.AnyAsync(s => s.Employee.SchoolID == schoolID);

            return await _context.LeaveRequests
                 .Include(tr => tr.Document)
                 .Include(tr => tr.Employee)
                 .ThenInclude(tr => tr.EmployeeType)
                 .Include(tr => tr.RequestedByUser)
                 .Include(tr => tr.ApprovedByUser)
                 .Include(tr => tr.Status)
                 .Where(e => e.Employee.SchoolID == schoolID &&
                 /*e.Status != null &&
                 e.Employee.Status.StatusType == "Employee" &&
                 e.Employee.Status.StatusText == "Active" &&*/
                 e.Employee.EmployeeType.Employeetype != null &&
                 e.Employee.EmployeeType.Employeetype == "Teaching Staff"
                 ) // Filter for Teaching Staff
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetNonTeachersLeaveRequestsBySchoolIDAsync(int schoolID)
        {
            bool schoolExists = await _context.Employees.AnyAsync(s => s.SchoolID == schoolID);
            if (!schoolExists)
            {
                return null;
            }
            var Exists = await _context.LeaveRequests.AnyAsync(s => s.Employee.SchoolID == schoolID);

            return await _context.LeaveRequests
                 .Include(tr => tr.Document)
                 .Include(tr => tr.Employee)
                 .ThenInclude(tr => tr.EmployeeType)
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
        public async Task<bool> DeleteLeaveRequestsAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                   .Where(lr => lr.LeaveRequestID == id)
                   .FirstOrDefaultAsync();

            if (leaveRequest == null)
            {
                throw new ArgumentException("Leave request with the given ID not found.");
            }

            var pendingStatusId = await _context.Statuses
      .Where(s => s.StatusText == "Pending" && s.StatusType == "LeaveRequest")
      .Select(s => s.StatusID)
      .FirstOrDefaultAsync();

            // Ensure the leave request has a "Pending" status
            if (leaveRequest.StatusID != pendingStatusId)
            {
                throw new ArgumentException("Leave request is not in pending status.");
            }

            // Get Active and Leave Status IDs for Employees
            var validEmployeeStatusIds = await _context.Statuses
                .Where(s => (s.StatusText == "Active" || s.StatusText == "Leave") && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .ToListAsync();

            // Check if a leave request exists with "Pending" status and the employee has "Active" or "Leave" status
            var isValidEmployee = await _context.Employees
      .AnyAsync(e => e.EmployeeID == leaveRequest.EmployeeID
                 && validEmployeeStatusIds.Contains(e.StatusID ?? 0));

            if (!isValidEmployee)
            {
                throw new ArgumentException("Employee is not in 'Active' or 'Leave' status.");
            }

            var LeaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (LeaveRequest == null) return false;

            _context.LeaveRequests.Remove(LeaveRequest);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<LeaveRequest?> EditLeaveRequestsAsync(int LeaveRequestId, EditLeaveRequestDTO editLeaveRequestDTO)
        {
            var leaveRequest = await _context.LeaveRequests
                 .Where(lr => lr.LeaveRequestID == LeaveRequestId)
                 .FirstOrDefaultAsync();

            if (leaveRequest == null)
            {
                throw new ArgumentException("Leave request with the given ID not found.");
            }

            var pendingStatusId = await _context.Statuses
      .Where(s => s.StatusText == "Pending" && s.StatusType == "LeaveRequest")
      .Select(s => s.StatusID)
      .FirstOrDefaultAsync();

            if (leaveRequest.StatusID != pendingStatusId)
            {
                throw new ArgumentException("Leave request is not in pending status.");
            }

            // Get Active and Leave Status IDs for Employees
            var validEmployeeStatusIds = await _context.Statuses
                .Where(s => (s.StatusText == "Active" || s.StatusText == "Leave") && s.StatusType == "Employee")
                .Select(s => s.StatusID)
                .ToListAsync();

            // Check if a leave request exists with "Pending" status and the employee has "Active" or "Leave" status
            var isValidEmployee = await _context.Employees
        .AnyAsync(e => e.EmployeeID == leaveRequest.EmployeeID
                   && validEmployeeStatusIds.Contains(e.StatusID ?? 0));

            if (!isValidEmployee)
            {
                throw new ArgumentException("Employee is not in 'Active' or 'Leave' status.");
            }



            // Update the existing user with the new values from DTO
            editLeaveRequestDTO.ToEditLeaveRequests(leaveRequest);

            _context.LeaveRequests.Update(leaveRequest);


            await _context.SaveChangesAsync();

            return leaveRequest;
        }
    }
}
