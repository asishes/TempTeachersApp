using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Services.Repositories
{
    public class TeacherHistoryService : ITeacherHistoryService
    {

        private readonly TeachersAppDbcontext _context;

        public TeacherHistoryService(TeachersAppDbcontext context)
        {
            _context = context;
        }


        public async Task<TeacherHistory> CreateTeacherHistoryAsync(TeacherHistory teacherHistory)
        {
            _context.TeacherHistories.Add(teacherHistory);
            await _context.SaveChangesAsync();
            return teacherHistory;
        }

        public async Task<List<TeacherHistory>> GetHistoriesByIdAsync( int EmployeeID)
        {
            try
            {
                var teacherHistories = await _context.TeacherHistories
                .Include(th => th.Employee)
                .ThenInclude(th => th.ApprovalType)
                .Include(th => th.ChangedByUser)
                .Include(th => th.ChangeType)
                .Include(th => th.SchoolFrom)
                .Include(th => th.SchoolTo)
                .Include(th => th.PromotedFromDesignation)
                .Include(th => th.PromotedToDesignation)
                .Where(th => th.EmployeeID == EmployeeID)
                .ToListAsync();

                return teacherHistories;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving Histories.", ex);
            }
        }

        public async Task<int> GetChangeTypeIdAsync(string changeTypeName)
        {
            return await _context.ChangeTypes
                .Where(ct => ct.ChangeText == changeTypeName)
                .Select(ct => ct.ChangeTypeID)
                .FirstOrDefaultAsync();
        }

        public async Task HandleTeacherHistoryAsync(Employee employee)
        {
            var teacherHistory = new TeacherHistory
            {
                EmployeeID = employee.EmployeeID,
                ChangeDate = employee.WorkStartDate,
                ChangeDescription = "Joined to " + (employee.Designation?.DesignationText ?? string.Empty),
                ChangeTypeID = await GetChangeTypeIdAsync("Joined"),
                ChangeFromSchoolID = null,
                ChangeToSchoolID = employee.SchoolID,
                PromotedFromPositionID = null,
                PromotedToPositionID = employee.DesignationID
            };

            await CreateTeacherHistoryAsync(teacherHistory);
            await _context.SaveChangesAsync();
        }
    }
}
