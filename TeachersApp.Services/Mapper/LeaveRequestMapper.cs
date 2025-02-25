using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class LeaveRequestMapper
    {
        public static LeaveRequest ToCreateLeaveRequest(this CreateLeaveRequestDTO createLeaveRequest)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeID = createLeaveRequest.EmployeeID,
                FromDate = createLeaveRequest.FromDate,
                ToDate = createLeaveRequest.ToDate,
                RequestorComment = createLeaveRequest.RequestorComment,
                DocumentID = createLeaveRequest.DocumentID,
            };
            return leaveRequest;
        }


        public static GetLeaveRequestDTO ToGetLeaveRequestDTO(this LeaveRequest leaveRequest)
        {
            return new GetLeaveRequestDTO
            {
                LeaveRequestID = leaveRequest.LeaveRequestID,
                EmployeeID = leaveRequest.EmployeeID,
                EmployeeName = $"{leaveRequest.Employee.FirstName ?? "Unknown"} {leaveRequest.Employee.LastName ?? "Unknown"}",
                FromDate = leaveRequest.FromDate,
                ToDate = leaveRequest.ToDate,
                RequestDate = leaveRequest.RequestDate,
                StatusChangeDate = leaveRequest.StatusChangeDate,
                ApprovalStatus = leaveRequest.StatusID,
                Status = leaveRequest.Status?.StatusText ?? string.Empty,
                RequestedByID = leaveRequest.RequestedByID ?? 0,
                RequestedByUser = leaveRequest.RequestedByUser?.FirstName ?? string.Empty,
                ApprovedByID = leaveRequest.ApprovedByID ?? 0,
                ApprovedByUser = leaveRequest.ApprovedByUser?.FirstName ?? string.Empty,
                RequestorComment = leaveRequest.RequestorComment ?? string.Empty,
                ApproverComment = leaveRequest.ApproverComment ?? string.Empty,
                DocumentID = leaveRequest.DocumentID,
                Documentpath = leaveRequest.Document?.DocumentFileName ?? null,
            };

        }

        public static void ToApproveLeave(this LeaveRequest transferRequest, LeaveRequest updateRequest)
        {          

            if (!string.IsNullOrWhiteSpace(updateRequest.ApproverComment))
                transferRequest.ApproverComment = updateRequest.ApproverComment;
    
        }
        public static LeaveRequest ToRejectLeaveRequest(this RejectLeaveRequestDTO rejectLeaveRequest)
        {
            return new LeaveRequest
            {
                ApproverComment = rejectLeaveRequest.ApproverComment,
            };
        }
    }
}
