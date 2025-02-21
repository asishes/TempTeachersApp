using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolTypeController : ControllerBase
    {
        private readonly ISchoolTypeService _schoolTypeService;


        public SchoolTypeController(ISchoolTypeService schoolTypeService)
        {
            _schoolTypeService = schoolTypeService;
        }

        [HttpPost("AddSchoolTypeWithDivision")]
        public async Task<IActionResult> CreateSchoolType([FromBody] AddSchoolTypeDTO addSchoolTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var schoolType = addSchoolTypeDTO.AddSchoolType();
            var result = await _schoolTypeService.CreateSchoolTypeAsync(schoolType);

            if (result is null) return NotFound("Please check the fields are valid");

            return Ok(result.GetSchoolType());
        }


        [HttpGet("GetSchoolTypeById{id}")]
        public async Task<IActionResult> GetSchoolTypeById(int id)
        {
            try
            {
                var schoolType = await _schoolTypeService.GetSchoolTypeByIdAsync(id);
                if (schoolType == null)
                {
                    return NotFound();
                }
                return Ok(schoolType);
            }

            catch (Exception)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        [HttpGet("GetAllSchoolTypes")]
        public async Task<ActionResult<List<GetAllSchoolTypesDTO>>> GetAllSchoolTypes()
        {
            try
            {
                // Call the service to get all school types
                var schoolTypes = await _schoolTypeService.GetAllSchoolTypesAsync();

                if (schoolTypes == null || schoolTypes.Count == 0)
                {
                    return NotFound("No school types found.");
                }

                // Return the list of school types as a 200 OK response
                return Ok(schoolTypes);
            }
            catch (Exception)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetClassesBySchoolType")]
        public async Task<IActionResult> GetClassesBySchoolType([FromQuery] List<int> schooltypeIDs)
        {
            try
            {
                if (schooltypeIDs == null || !schooltypeIDs.Any())
                {
                    return BadRequest("At least one school type ID must be provided.");
                }

                // Fetch the list of SchoolType from the service
                var schoolTypes = await _schoolTypeService.GetClassesBySchoolTypeAsync(schooltypeIDs);

                // Transform each SchoolType into GetClassesBySchoolTypeDTO
                var classRanges = schoolTypes
                    .SelectMany(schoolType => schoolType.ToGetClasses().Classes)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                // If no classes are found, return NotFound
                if (!classRanges.Any())
                {
                    return NotFound($"No classes found for the provided school type IDs: {string.Join(", ", schooltypeIDs)}.");
                }

                // Return the result
                return Ok(new { Classes = classRanges });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
