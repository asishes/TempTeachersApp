using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest request);

        Task<List<LeaveRequest>> GetAllTeacherLeaveRequestAsync();

        Task<List<LeaveRequest>> GetAllNonTeacherLeaveRequestAsync();

       

        Task<LeaveRequest?> ApproveLeaveRequestAsync(int Id, ApproveLeaveRequestDTO leaveRequest);

        Task<LeaveRequest?> RejectLeaveRequestAsync(int Id, LeaveRequest leaveRequest);
        Task<List<LeaveRequest>> FilterNonTeacherLeaveCompletedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null
           );

        Task<List<LeaveRequest>> FilterTeacherLeaveCompletedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null
           );
        Task<List<LeaveRequest>> FilterNonTeacherLeaveRequestedListAsync(
         int? schoolID = null,
         int? designationID = null,
         string? uniqueID = null
         );

        Task<List<LeaveRequest>> FilterTeacherLeaveRequestedListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null
           );




        Task<List<LeaveRequest>> GetAllLeaveNonTeacherAsync();

        Task<List<LeaveRequest>> GetAllLeaveTeacherAsync();

        Task<LeaveRequest?> ApproveLeaveRequestByHeadMasterAsync(int Id);
        Task<List<LeaveRequest>> GetLeaveRequestsByEmployeeIdAsync(int employeeID);
        Task<List<LeaveRequest>> GetTeachersLeaveRequestsBySchoolIDAsync(int schoolID);

        Task<List<LeaveRequest>> GetNonTeachersLeaveRequestsBySchoolIDAsync(int schoolID);
    }
}
