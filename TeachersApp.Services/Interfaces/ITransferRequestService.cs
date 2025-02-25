using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ITransferRequestService
    {
        Task<TransferRequest> CreateTransferRequestAsync(TransferRequest request);

        Task<List<TransferRequest>> GetAllTeacherTransferRequestAsync();

        Task<List<TransferRequest>> GetAllNonTeacherTransferRequestAsync();


        Task<TransferRequest?> ApproveTransferAsync(int Id, ApproveTransferRequestDTO transferRequest);

        Task<TransferRequest?> RejectTransferAsync(int Id, TransferRequest transferRequest);

        Task<List<TransferRequest>> FilterTeacherTransferCompletedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromTransferDate = null,
           DateTime? toTransferDate = null,
           DateTime? fromWitheffectDate = null,
           DateTime? toWIthEffectDate = null
           );

        Task<List<TransferRequest>> FilterNonTeacherTransferCompletedListAsync(
          int? schoolID = null,
          int? designationID = null,
          string? uniqueID = null,
          DateTime? fromTransferDate = null,
          DateTime? toTransferDate = null,
          DateTime? fromWitheffectDate = null,
          DateTime? toWIthEffectDate = null
          );
            
        Task<List<TransferRequest>> FilterTeacherTransferRequestListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromTransferDate = null,
           DateTime? toTransferDate = null
           );

        Task<List<TransferRequest>> FilterNonTeacherTransferRequestListAsync(
          int? schoolID = null,
          int? designationID = null,
          string? uniqueID = null,
          DateTime? fromTransferDate = null,
          DateTime? toTransferDate = null
          );

        Task<List<TransferRequest>> GetAllTransferedNonTeacherAsync();

        Task<List<TransferRequest>> GetAllTransferedTeacherAsync();

        Task<List<TransferRequest>> GetTransferRequestByEmployeeIdAsync(int employeeID);

        Task<List<TransferRequest>> GetTeachersTransferRequestsBySchoolIDAsync(int schoolID);

        Task<List<TransferRequest>> GetNonTeachersTransferRequestsBySchoolIDAsync(int schoolID);
    }
}
