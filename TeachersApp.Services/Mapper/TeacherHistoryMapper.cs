using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.SchoolClassDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.ModelDTO.TeacherHistoryDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class TeacherHistoryMapper
    {

        public static TeacherHistory ToAddTeacherHistory(this CreateTeacherHistoryDTO createTeacherHistoryDTO)
        {
            var teacherHistory = new TeacherHistory
            {
                EmployeeID = createTeacherHistoryDTO.EmployeeID,
                ChangeDate = createTeacherHistoryDTO.ChangeDate,
                ChangedByID = createTeacherHistoryDTO.ChangedByID,
                ChangeDescription = createTeacherHistoryDTO.ChangeDescription,
                ChangeTypeID = createTeacherHistoryDTO.ChangeTypeID,
                ChangeFromSchoolID = createTeacherHistoryDTO.ChangeFromSchoolID,
                ChangeToSchoolID = createTeacherHistoryDTO.ChangeToSchoolID,
                PromotedFromPositionID = createTeacherHistoryDTO.PromotedFromPositionID,
                PromotedToPositionID = createTeacherHistoryDTO.PromotedToPositionID
            };
            return teacherHistory;
        }

        public static GetTeacherHistoryDTO ToGetTeacherHistoryDTO(this TeacherHistory teacherHistory)
        {
            return new GetTeacherHistoryDTO
            {
                HistoryID = teacherHistory.HistoryID,
                EmployeeID = teacherHistory.Employee?.EmployeeID ?? 0, 
                DateofJoin = teacherHistory.Employee?.WorkStartDate,
                RetireDate = teacherHistory.Employee?.RetirementDate, 
                ChangeDate = teacherHistory.ChangeDate, 
                ApprovalType = teacherHistory.Employee?.ApprovalTypeID, 
                ChangedByID = teacherHistory.ChangedByID,
                ChangeDescription = teacherHistory.ChangeDescription ?? string.Empty, 
                ChangeTypeID = teacherHistory.ChangeTypeID,
                ChangeFromSchoolID = teacherHistory.ChangeFromSchoolID,
                ChangeToSchoolID = teacherHistory.ChangeToSchoolID,
                PromotedFromPositionID = teacherHistory.PromotedFromPositionID,
                PromotedToPositionID = teacherHistory.PromotedToPositionID,

                // Safe Null Handling for Strings
                EmployeeName = $"{teacherHistory.Employee?.FirstName ?? string.Empty} {teacherHistory.Employee?.LastName ?? string.Empty}".Trim(),
                Username = teacherHistory.ChangedByUser != null
            ? $"{teacherHistory.ChangedByUser?.FirstName ?? string.Empty} {teacherHistory.ChangedByUser?.LastName ?? string.Empty}".Trim()
            : string.Empty,
                ChangedtypeName = teacherHistory.ChangeType?.ChangeText ?? string.Empty,
                ApprovalTypeText = teacherHistory.Employee?.ApprovalType?.Approvaltype ?? string.Empty,
                FromSchoolName = teacherHistory.SchoolFrom?.SchoolName ?? string.Empty,
                ToSchoolName = teacherHistory.SchoolTo?.SchoolName ?? string.Empty,
                FromPosition = teacherHistory.PromotedFromDesignation?.DesignationText ?? string.Empty,
                ToPosition = teacherHistory.PromotedToDesignation?.DesignationText ?? string.Empty
            };
        }

    }
}
