using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO.PromotionRelinquishment;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Interfaces
{
    public interface IPromotionService
    {
        Task UpdatePromotionSeniorityAsync();

        Task<List<Employee>> GetSortedPromotionEligibleEmployeesAsync();


        Task<IEnumerable<GetPromotionListDTO>> GetEmployeesByDesignationAndSubjectAsync(int designationId, int? subjectId);

        Task<Promotion> CreatePromotionRequestAsync(Promotion request);

        Task<List<Promotion>> GetAllTeacherPromotionRequestAsync();

        Task<List<Promotion>> GetAllNonTeacherPromotionRequestAsync();


        Task<Promotion?> ApprovePromotionRequestAsync(int Id, ApprovePromotionRequestDTO promotionRequest);

        Task<Promotion?> RejectPromotionRequestAsync(int Id, Promotion promotionRequest);

        Task<List<Promotion>> FilterTeacherPromotionCompletedListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null,
            DateTime? fromPromotionDate = null,
            DateTime? toPromotionDate = null
            );

        Task<List<Promotion>> FilterNonTeacherPromotionCompletedListAsync(
            int? schoolID = null,
            int? designationID = null,
            string? uniqueID = null,
            DateTime? fromPromotionDate = null,
            DateTime? toPromotionDate = null
            );

        Task<List<Promotion>> FilterTeacherPromotionRequestListAsync(
           int? schoolID = null,
           int? designationID = null,
           string? uniqueID = null,
           DateTime? fromPromotionDate = null,
           DateTime? toPromotionDate = null
           );

        Task<List<Promotion>> FilterNonTeacherPromotionRequestListAsync(
          int? schoolID = null,
          int? designationID = null,
          string? uniqueID = null,
          DateTime? fromPromotionDate = null,
          DateTime? toPromotionDate = null
          );

        Task<List<Promotion>> GetAllPromotedNonTeacherAsync();

        Task<List<Promotion>> GetAllPromotedTeacherAsync();

        Task<bool> PromotionEligible(Employee employee);

        Task<PromotionRelinquishment> CreatePromotionRelinquishment(PromotionRelinquishment promotionRelinquishment);
    
        Task<List<PromotionRelinquishment>> GetAllPromotionRelinquishmentsAsync();

        Task<PromotionRelinquishment?> ApprovePromotionRelinquishmentAsync(int id, ApprovePromotionRelinquishmentDTO updateDTO);

        Task<List<Promotion>> GetPromotionRequestByEmployeeIdAsync(int employeeID);
        
        Task<List<Promotion>> GetTeachersPromotionRequestsBySchoolIDAsync(int schoolID);
    }
}
