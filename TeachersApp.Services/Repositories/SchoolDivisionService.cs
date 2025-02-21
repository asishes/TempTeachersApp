using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Services.Repositories
{
    public class SchoolDivisionService : ISchoolDivisionService
    {
        private readonly TeachersAppDbcontext _context;

        public SchoolDivisionService(TeachersAppDbcontext context)
        {
            _context = context;
        }

        public async Task<SchoolDivisionCount> AddSchoolDivisionAsync(SchoolDivisionCount schoolDivision)
        {
            _context.SchoolDivisionCounts.Add(schoolDivision);
            await _context.SaveChangesAsync();
            return schoolDivision;
        }

        public async Task<GetSchoolDivisionDTO> GetSchoolDivisionByIdAsync(int schoolDivisionId)
        {
            try
            {
                var schoolDivision = await _context.SchoolDivisionCounts.FindAsync(schoolDivisionId);
                if (schoolDivision == null)
                {
                    throw new Exception("School Division not found");
                }
                return schoolDivision.GetSchoolDivision();
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to retrieve school division.", ex);
            }
        }
    }
}
