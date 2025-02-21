using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.PositionDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;

namespace TeachersApp.Services.Repositories
{
    public class PositionService : IPositionService
    {
        public readonly TeachersAppDbcontext _context;

        public PositionService(TeachersAppDbcontext context)
        {
            _context = context;
        }

        // Get Total Vacant Position Count

        #region GetTotalVacantPositionCount

        public async Task<int> GetTotalVacantPositionCountAsync()
        {
            try
            {
                return await _context.SchoolPositions
                     .Where(s =>
                     (s.Status.StatusText == "New" || s.Status.StatusText == "Vacant") &&
                      s.Status.StatusType == "Position")
                     .CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve vacant position count.", ex);
            }
        }

        #endregion

        public async Task<SchoolPosition> CreateNewPositionAsync(SchoolPosition schoolPosition)
        {
            schoolPosition.StatusID = await _context.Statuses
                .Where(s => s.StatusText == "New" && s.StatusType == "Position")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();
            await _context.SchoolPositions.AddAsync(schoolPosition);
            await _context.SaveChangesAsync();
            return schoolPosition;


        }

        public async Task<List<SchoolPosition>> GetAllVacantAndNewSchoolPositionsAsync()
        {
            return await _context.SchoolPositions
                .Include(sp => sp.School)
                .ThenInclude(s => s.City)
                .Include(sp => sp.Subject)
                .Include(sp => sp.Designation)
                .Include(sp => sp.Status)
                .Where(s =>
                    (s.Status.StatusText == "New" || s.Status.StatusText == "Vacant") &&
                    s.Status.StatusType == "Position")
                .ToListAsync();


        }

        public async Task<int> HandleVacanciesForEmployeesAsync()
        {
            int createdVacancies = 0;

            createdVacancies += await HandleVacanciesForRetirementAsync();
            createdVacancies += await HandleVacanciesForPromotionsAsync();
            createdVacancies += await HandleVacanciesForTransfersAsync();

            return createdVacancies;
        }

        private async Task<int> HandleVacanciesForRetirementAsync()
        {
            var retiredStatusId = await GetStatusIdAsync("Retired", "Employee");

            var retiredEmployees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.StatusID == retiredStatusId)
                .ToListAsync();

            if (!retiredEmployees.Any())
            {
                return 0;
            }

            var vacantStatusId = await GetStatusIdAsync("Vacant", "Position");

            int createdVacancies = 0;

            foreach (var retiredEmployee in retiredEmployees)
            {
                var existingVacancy = await _context.SchoolPositions
                    .AsNoTracking()
                    .Where(v => v.DesignationID == retiredEmployee.DesignationID &&
                                v.SubjectID == retiredEmployee.SubjectID &&
                                v.SchoolID == retiredEmployee.SchoolID &&
                                v.StatusID == vacantStatusId)
                    .FirstOrDefaultAsync();

                if (existingVacancy == null)
                {
                    var newVacancy = new SchoolPosition
                    {
                        DesignationID = retiredEmployee.DesignationID ?? 0,
                        SubjectID = retiredEmployee.SubjectID ?? 0,
                        SchoolID = retiredEmployee.SchoolID ?? 0,
                        StatusID = vacantStatusId
                    };

                    await CreateVacancyAsync(newVacancy);
                    createdVacancies++;
                }
            }

            return createdVacancies;
        }

        private async Task<int> HandleVacanciesForPromotionsAsync()
        {
            var promotedEmployees = await _context.Promotions
                .AsNoTracking()
                .Where(pr => pr.Status.StatusType == "Promotion" &&
                             pr.Status.StatusText == "Completed")
                .Select(pr => new
                {
                    Employee = pr.Employee,
                    PromotedFromDesignationID = pr.PromotedFromDesignationID,
                    FromSchoolID = pr.FromSchoolID
                })
                .ToListAsync();

            if (!promotedEmployees.Any())
            {
                return 0;
            }

            var vacantStatusId = await GetStatusIdAsync("Vacant", "Position");

            int createdVacancies = 0;

            foreach (var promotion in promotedEmployees)
            {
                var existingVacancy = await _context.SchoolPositions
                    .AsNoTracking()
                    .Where(v => v.DesignationID == promotion.PromotedFromDesignationID &&
                                v.SubjectID == promotion.Employee.SubjectID &&
                                v.SchoolID == promotion.FromSchoolID &&
                                v.StatusID == vacantStatusId)
                    .FirstOrDefaultAsync();

                if (existingVacancy == null)
                {
                    var newVacancy = new SchoolPosition
                    {
                        DesignationID = promotion.PromotedFromDesignationID,
                        SubjectID = promotion.Employee.SubjectID ?? 0,
                        SchoolID = promotion.FromSchoolID ?? 0,
                        StatusID = vacantStatusId
                    };

                    await CreateVacancyAsync(newVacancy);
                    createdVacancies++;
                }
            }

            return createdVacancies;
        }

        private async Task<int> HandleVacanciesForTransfersAsync()
        {
            var transferredEmployees = await _context.TransferRequests
                .AsNoTracking()
                .Where(tr => tr.Status.StatusType == "TransferRequest" &&
                             tr.Status.StatusText == "Completed")
                .Select(tr => new
                {
                    OldSchoolID = tr.FromSchoolID,
                    Employee = tr.Employee
                })
                .ToListAsync();

            if (!transferredEmployees.Any())
            {
                return 0;
            }

            var vacantStatusId = await GetStatusIdAsync("Vacant", "Position");

            int createdVacancies = 0;

            foreach (var transfer in transferredEmployees)
            {
                var existingVacancy = await _context.SchoolPositions
                    .AsNoTracking()
                    .Where(v => v.DesignationID == transfer.Employee.DesignationID &&
                                v.SubjectID == transfer.Employee.SubjectID &&
                                v.SchoolID == transfer.OldSchoolID &&
                                v.StatusID == vacantStatusId)
                    .FirstOrDefaultAsync();

                if (existingVacancy == null)
                {
                    var newVacancy = new SchoolPosition
                    {
                        DesignationID = transfer.Employee.DesignationID ?? 0,
                        SubjectID = transfer.Employee.SubjectID ?? 0,
                        SchoolID = transfer.OldSchoolID,
                        StatusID = vacantStatusId
                    };

                    await CreateVacancyAsync(newVacancy);
                    createdVacancies++;
                }
            }

            return createdVacancies;
        }

        private async Task<int> GetStatusIdAsync(string statusText, string statusType)
        {
            return await _context.Statuses
                .AsNoTracking()
                .Where(s => s.StatusText == statusText && s.StatusType == statusType)
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();
        }

        public async Task<SchoolPosition> CreateVacancyAsync(SchoolPosition schoolPosition)
        {
            _context.SchoolPositions.Add(schoolPosition);
            await _context.SaveChangesAsync();
            return schoolPosition;
        }

        public async Task<bool> DeleteSchoolPositionAsync(int id)
        {
            var schoolPosition = await _context.SchoolPositions.FindAsync(id);
            if (schoolPosition == null) return false;

            _context.SchoolPositions.Remove(schoolPosition);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
