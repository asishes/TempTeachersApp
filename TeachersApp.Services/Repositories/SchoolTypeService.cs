using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;

namespace TeachersApp.Services.Repositories
{
    public class SchoolTypeService : ISchoolTypeService
    {
        private readonly TeachersAppDbcontext _context;


        public SchoolTypeService(TeachersAppDbcontext context)
        {
            _context = context;
        }

        public async Task<SchoolType> CreateSchoolTypeAsync(SchoolType schoolType)
        {
            await _context.SchoolTypes.AddAsync(schoolType);
            await _context.SaveChangesAsync();
            return schoolType;

            
        }

        public async Task<GetSchoolTypeDTO> GetSchoolTypeByIdAsync(int schoolTypeId)
        {
            try
            {
                var schoolType = await _context.SchoolTypes.FindAsync(schoolTypeId);
                if (schoolType == null)
                {
                    throw new Exception("School Type not found");
                }
                return schoolType.GetSchoolType();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve schooltype.", ex);
            }
        }

        public async Task<List<GetAllSchoolTypesDTO>> GetAllSchoolTypesAsync()
        {
            try
            {
                var schoolTypes = await _context.SchoolTypes.ToListAsync();
                return schoolTypes.Select(schoolType => schoolType.ToGetAllSchoolTypes()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve all schooltypes.", ex);
            }

        }

        public async Task<List<SchoolType>> GetClassesBySchoolTypeAsync(List<int> schooltypeIDs)
        {
            // Fetch the list of SchoolTypes filtered by the provided IDs
            var schoolTypes = await _context.SchoolTypes
                .Where(st => schooltypeIDs.Contains(st.SchoolTypeID))
                .ToListAsync();

            // Return the list (even if empty)
            return schoolTypes;
        }

    }
}
