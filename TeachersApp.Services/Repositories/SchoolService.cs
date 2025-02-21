using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TeachersApp.Services.Repositories
{
    public class SchoolService : ISchoolService

    {
        private readonly TeachersAppDbcontext _context;
        private readonly IPersonalDetailsService _personalDetailsService;
        
        public SchoolService(TeachersAppDbcontext context,IPersonalDetailsService personalDetailsService)
        {
            _context = context;
            _personalDetailsService = personalDetailsService;
        }

        // Get Total Open School Count - 2

        #region GetTotalOpenSchoolCount

        public async Task<int> GetTotalOpenSchoolCountAsync()
        {
            try
            {
                return await _context.Schools
                .Where(s => s.Status.StatusText == "Open") // Accessing StatusText through the navigation property
                .CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve open school count .", ex);
            }
        }
        #endregion





        #region GetSchoolPopUp

        public async Task<SchoolPopUpDTO> GetSchoolPopUpAsync(int schoolId)
        {
            try
            {
                var school = await _context.Schools
                    .Include(s => s.Photo)
                    .Include(s => s.City)
                    .Include(s => s.Principal)
                    .ThenInclude(s => s.Photo)
                    .Include(s => s.VicePrincipal)
                    .ThenInclude(s => s.Photo)
                     .FirstOrDefaultAsync(s => s.SchoolID == schoolId);
                if (school == null)
                {
                    // Handle case where school is not found
                    throw new ArgumentException($"School with ID {schoolId} not found.");
                }

                // Map School entity to GetSchoolDTO using extension method
                return school.SchoolPopUpDTO();
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving school details.", ex);
            }
        }


        #endregion


        #region GetSchoolListAsync
        public async Task<List<School>> GetSchoolListAsync()
        {
            try
            {
                var schoolsList = await _context.Schools
                    .Include(s => s.SchoolClasses)
                    .ThenInclude(sc => sc.SchoolDivisionCounts)
                    .Include(s => s.SchoolStandardTypes)
                    .ThenInclude(s => s.SchoolType)
                    .Include(s => s.Principal)
                    .Include (s => s.City)
                    .Include(s => s.Employees)
                    .Include(s => s.SchoolClasses)
                    .Where(s => s.Status.StatusText == "Open" && s.Status.StatusType == "School")
                    .ToListAsync();

                if (!schoolsList.Any())
                {
                    throw new ArgumentException("No schools found.");
                }

                return schoolsList;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving schools.", ex);
            }
        }
        #endregion


        #region GetSchoolListWithOrWithoutID

        public async Task<List<SchoolDTO>> GetSchoolListIdAsync(int? schoolId = null)
        {
            var query = _context.Schools.AsQueryable();

            if (schoolId.HasValue)
            {
                query = query.Where(s => s.SchoolID == schoolId.Value);
            }

            // Apply the status filter for "Open" schools
            query = query.Where(s => s.Status.StatusText == "Open");

            return await query.Select(s => new SchoolDTO
            {
                SchoolID = s.SchoolID,
                SchoolName = s.SchoolName,
                Address = s.Address,
                CityName = s.City.CityName,
                State = s.State,
                Pincode = s.Pincode,
                Email = s.Email,
                ContactNumber = s.Phone,
                // Fetch names of Principal and Vice Principal
                PrincipalName = s.Principal.FirstName + " " + s.Principal.LastName,
                VicePrincipalName = s.VicePrincipal.FirstName + " " + s.VicePrincipal.LastName
            }).ToListAsync();
        }

        #endregion



        public async Task<School> AddSchoolAsync(School school)
        {

         

            school.StatusID = await _context.Statuses
              .Where(s => s.StatusText == "Open" && s.StatusType == "School")
              .Select(s => s.StatusID)
              .FirstOrDefaultAsync();



            await _context.Schools.AddAsync(school);
            await _context.SaveChangesAsync();
            return school;
        }

        public async Task<School> GetSchoolHomePageAsync(int schoolId)
        {
            try
            {
                var school = await _context.Schools
                    .Include(s => s.SchoolClasses)
                    .ThenInclude(sc => sc.SchoolDivisionCounts)
                    .Include(s => s.SchoolStandardTypes)
                    .ThenInclude(s => s.SchoolType)
                    .Include(s => s.City)
                    .Include(s => s.Photo)
                    .Include(s => s.Principal)
                    .Include(s => s.VicePrincipal)
                    .Include(s => s.SchoolClasses)
                    .Include(s => s.Status)
                    .FirstOrDefaultAsync(s => s.SchoolID == schoolId);

                if (school == null)
                {
                    throw new ArgumentException($"School with ID {schoolId} not found.");
                }

                return school; // Return the School entity directly
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving school details.", ex);
            }
        }



        public async Task<GetSchoolDTO> GetSchoolAsync(int schoolId)
        {
            try
            {
                var school = await _context.Schools
                    .Include(s => s.SchoolClasses)
                    .ThenInclude(sc => sc.SchoolDivisionCounts)
                    .Include(s => s.SchoolStandardTypes)
                    .ThenInclude(s => s.SchoolType)
                    .Include(s => s.City)
                    .Include(s => s.Photo)
                    .Include(s => s.Principal)
                    .Include(s => s.VicePrincipal)
                    .Include(s => s.Status)
                    .FirstOrDefaultAsync(s => s.SchoolID == schoolId);

                if (school == null)
                {
                    // Handle case where school is not found
                    throw new ArgumentException($"School with ID {schoolId} not found.");
                }

                // Map School entity to GetSchoolDTO using extension method
                return school.ToGetSchoolDTO();
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving school details.", ex);
            }
        }


        public async Task<IEnumerable<GetAllSchoolsWithCityDTO>> GetSchoolsWithCityAsync()
        {
            try
            {
                var schools = await _context.Schools
                    .Include(s => s.City)
                    .Where(s => s.Status.StatusText == "Open" && s.Status.StatusType == "School")
                    .ToListAsync(); // Execute the query asynchronously.

                if (schools == null || !schools.Any())
                {
                    // Handle case where no schools are found
                    throw new ArgumentException("No schools found.");
                }

                // Map School entities to GetAllSchoolsWithCityDTO using the extension method
                return schools.Select(school => school.ToGetSchoolWithCity());
            }
            catch (Exception ex)
            {
                // Handle exceptions according to your application's error handling strategy
                throw new ApplicationException("Error retrieving schools.", ex);
            }
        }

        public async Task<School?> UpdateSchoolAsync(int id, School schoolUpdate)
        {
            var school = await _context.Schools
                .Include(s => s.SchoolStandardTypes)
                .Include(s => s.SchoolClasses)
                .ThenInclude(sc => sc.SchoolDivisionCounts)
                .FirstOrDefaultAsync(s => s.SchoolID == id);

            if (school == null)
            {
                return null;
            }

            // Update school properties
            school.SchoolName = schoolUpdate.SchoolName;
            // Update SchoolClasses and Divisions
            if (schoolUpdate.SchoolStandardTypes != null)
            {
                school.SchoolStandardTypes.Clear(); // Clear existing classes
                foreach (var updatedtypes in schoolUpdate.SchoolStandardTypes)
                {
                    var newType = new SchoolStandardType
                    {
                        SchoolTypeID = updatedtypes.SchoolTypeID,
                    };
                    school.SchoolStandardTypes.Add(newType);
                }
            }

            school.Address = schoolUpdate.Address;
            school.CityID = schoolUpdate.CityID;
            school.State = schoolUpdate.State;
            school.Pincode = schoolUpdate.Pincode;
            school.Email = schoolUpdate.Email;
            school.Phone = schoolUpdate.Phone;
            school.PhotoID = schoolUpdate.PhotoID;
            school.PrincipalID = schoolUpdate.PrincipalID;
            school.VicePrincipalID = schoolUpdate.VicePrincipalID;

            // Update SchoolClasses and Divisions
            if (schoolUpdate.SchoolClasses != null)
            {
                school.SchoolClasses.Clear(); // Clear existing classes
                foreach (var updatedClass in schoolUpdate.SchoolClasses)
                {
                    var newClass = new SchoolClass
                    {
                        Class = updatedClass.Class,
                        SchoolDivisionCounts = updatedClass.SchoolDivisionCounts?.Select(d => new SchoolDivisionCount
                        {
                            Division = d.Division,
                            StudentCount = d.StudentCount
                        }).ToList() ?? new List<SchoolDivisionCount>()
                    };
                    school.SchoolClasses.Add(newClass);
                }
            }

            _context.Schools.Update(school);
            await _context.SaveChangesAsync();

            return school;
        }



        public async Task<List<School>> GetSchoolWithAuthorityListAsync()
        {
            try
            {
                var schoolsList = await _context.Schools
                    .Include(s => s.City)
                    .Include(s => s.Principal)
                    .Include(s => s.VicePrincipal)
                    .Where(s => s.Status.StatusText == "Open" && s.Status.StatusType == "School")
                    .ToListAsync();

                return schoolsList;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving schools.", ex);
            }
        }
    }
}
