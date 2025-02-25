using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolDivisionController : ControllerBase
    {
        private readonly ISchoolDivisionService _schoolDivisionService;

        public SchoolDivisionController(ISchoolDivisionService schoolDivisionService)
        {
            _schoolDivisionService = schoolDivisionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSchoolDivision([FromBody] AddSchoolDivisionDTO addSchoolDivisionDTO)
        {
            var schoolDivision = addSchoolDivisionDTO.AddSchoolDivision();
            var result = await _schoolDivisionService.AddSchoolDivisionAsync(schoolDivision);

            if (result is null) return NotFound("Please check the fields are valid");

            return Ok(result.GetSchoolDivision());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolDivisionById(int id)
        {
            try
            {
                var schoolDivision = await _schoolDivisionService.GetSchoolDivisionByIdAsync(id);
                if (schoolDivision == null)
                {
                    return NotFound("School Division not found");
                }
                return Ok(schoolDivision);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving the school division.");
            }
        }
    }
}
