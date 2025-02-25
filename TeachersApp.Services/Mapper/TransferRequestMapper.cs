using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class TransferRequestMapper
    {
        public static TransferRequest ToCreateTransferRequest( this CreateTransferRequestDTO createtransferRequest)
        {
            var transferRequest = new TransferRequest
            {
                EmployeeID = createtransferRequest.EmployeeID,
                ToSchoolID_One = createtransferRequest.ToSchoolIDOne,
                ToSchoolID_Two = createtransferRequest.ToSchoolIDTwo,
                ToSchoolID_Three = createtransferRequest.ToSchoolIDThree, 
                TransferDate = createtransferRequest.TransferDate,
                RequestorComment = createtransferRequest.RequestorComment,
                FilePath = createtransferRequest?.FilePath,     
            };
            return transferRequest;
        }


        public static GetTransferRequestDTO ToGetTransferRequestDTO(this TransferRequest transferRequest)
        {
            return new GetTransferRequestDTO
            {
                TransferRequestID = transferRequest.TransferRequestID,
                EmployeeID = transferRequest.EmployeeID,
                EmployeeName = $"{transferRequest.Employee.FirstName ?? string.Empty} {transferRequest.Employee.LastName ?? string.Empty}",
                DesignationID = transferRequest.Employee.DesignationID ??(int?) null,
                DesignationName = transferRequest.Employee.Designation.DesignationText?? string.Empty,
                FromSchoolID = transferRequest.FromSchoolID,
                FromSchoolName = transferRequest.FromSchool?.SchoolName ?? string.Empty,
                ToSchoolID_One = transferRequest.ToSchoolID_One,
                ToSchoolOneName = transferRequest.ToSchool_One?.SchoolName ?? string.Empty,
                ToSchoolID_Two = transferRequest.ToSchoolID_Two,
                ToSchoolTwoName = transferRequest.ToSchool_Two?.SchoolName ?? string.Empty,
                ToSchoolID_Three = transferRequest.ToSchoolID_Three,
                ToSchoolThreeName = transferRequest.ToSchool_Three?.SchoolName ?? string.Empty,
                ToApprovedSchoolID = transferRequest.ToSchoolIDApproved,
                ToApprovedSchoolName = transferRequest.ToSchoolApproved?.SchoolName ?? string.Empty,
                RequestDate = transferRequest.RequestDate,
                StatusChangeDate = transferRequest.StatusChangeDate,
                TransferDate = transferRequest.TransferDate,
                ApprovalStatus = transferRequest.StatusID,
                Status = transferRequest.Status?.StatusText ?? string.Empty,
                RequestedByID = transferRequest.RequestedByID ?? 0,
                RequestedByUser = transferRequest.RequestedByUser?.FirstName ?? string.Empty,
                ApprovedByID = transferRequest.ApprovedByID ?? 0,
                ApprovedByUser = transferRequest.ApprovedByUser?.FirstName ?? string.Empty,
                RequestorComment = transferRequest.RequestorComment ?? string.Empty,
                ApproverComment = transferRequest.ApproverComment ?? string.Empty,
                FilePath = transferRequest.FilePath ?? string.Empty
            };

        }

        public static void ToApproveTransfer(this TransferRequest transferRequest, TransferRequest updateRequest)
        {
            if (updateRequest.ToSchoolIDApproved.HasValue)
                transferRequest.ToSchoolIDApproved = updateRequest.ToSchoolIDApproved;

            if (updateRequest.TransferDate.HasValue)
                transferRequest.TransferDate = updateRequest.TransferDate;

            if (!string.IsNullOrWhiteSpace(updateRequest.ApproverComment))
                transferRequest.ApproverComment = updateRequest.ApproverComment;

        }
        public static TransferRequest ToRejectTransfer(this RejectTransferRequestDTO rejectTransfer)
        {
            return new TransferRequest
            {
                ApproverComment = rejectTransfer.ApproverComment,   
            };
        }

    }

}
