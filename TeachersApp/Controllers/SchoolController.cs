using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.PhotoDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.UserDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }


        #region GetTotalOpenSchoolCount

        [HttpGet("GetAllOpenSchoolsCount")]
        public async Task<ActionResult<int>> GetTotalOpenSchoolCount()
        {
            try
            {
                var count = await _schoolService.GetTotalOpenSchoolCountAsync();
                return Ok(count);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving the open school count.");
            }


        }
        #endregion


        #region SchoolPopUp

        [HttpGet("GetSchoolPopUp/{schoolId}")]
        public async Task<IActionResult> GetSchoolPopUp(int schoolId)
        {
           
            try
            {
                // Call the service method to get the school details
                var schoolDetails = await _schoolService.GetSchoolPopUpAsync(schoolId);

                if (schoolDetails == null)
                {
                    // Return 404 if the school is not found
                    return NotFound(new { message = $"School with ID {schoolId} not found." });
                }

                // Return 200 OK with the school details in the response
                return Ok(schoolDetails);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument exceptions (e.g., school not found)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving school details.", error = ex.Message });
            }
        }
        #endregion



        #region ListSchoolLists

        [HttpGet("GetOpenSchoolsList")]
        public async Task<ActionResult<List<SchoolListDTO>>> GetOpenSchoolsList()
        {
            try
            {
                // Call the service method to get the school details
                var schoolsList = await _schoolService.GetSchoolListAsync();

                if (!schoolsList.Any())
                {
                    // Return 404 if no schools are found
                    return NotFound(new { message = "Schools not found." });
                }

                // Return 200 OK with the school details in the response
                var schoolDtoList = schoolsList.Select(s => s.ToGetSchoolListDTO()).ToList();
                return Ok(schoolDtoList);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument exceptions (e.g., school not found)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving school details.", error = ex.Message });
            }
        }
        #endregion



        #region GetAllSchools

        [HttpGet]
        public async Task<ActionResult<List<SchoolDTO>>> GetAllSchools([FromQuery] int? schoolId = null)
        {
            try
            {
                var schools = await _schoolService.GetSchoolListIdAsync(schoolId);

                // Check if the list is empty and return a 404 if no schools found for a specific ID
                if (schoolId.HasValue && !schools.Any())
                {
                    return NotFound(new { Message = $"No school found with ID {schoolId.Value}" });
                }

                return Ok(schools);
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging mechanism
                // _logger.LogError(ex, "An error occurred while retrieving school data.");

                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
        #endregion


        [HttpPost("AddSchool")]
        public async Task<IActionResult> AddSchool([FromBody] AddSchoolDTO addSchoolDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var convertedSchool = addSchoolDTO.ToAddSchool();
            var newSchool = await _schoolService.AddSchoolAsync(convertedSchool);

            if (newSchool is null) return NotFound("Please check the fields are valid");

            return StatusCode(200, new { message = "School Created successfully", status = 200 });
        }

        [HttpGet("GetSchoolHomePage/{schoolId}")]
        public async Task<IActionResult> GetSchoolHomePage(int schoolId)
        {
            try
            {
                // Call the service method to get the School entity
                var school = await _schoolService.GetSchoolHomePageAsync(schoolId);

                if (school == null)
                {
                    return NotFound(new { message = $"School with ID {schoolId} not found." });
                }

                // Map School entity to GetSchoolDTO
                var schoolDto = school.ToGetSchoolDTO();

                return Ok(schoolDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving school details.", error = ex.Message });
            }
        }



        [HttpGet("GetSchool{schoolId}")]
        public async Task<IActionResult> GetSchoolDetails(int schoolId)
        {
            try
            {
                // Call the service method to get the school details
                var schoolDto = await _schoolService.GetSchoolAsync(schoolId);

                if (schoolDto == null)
                {
                    // Return 404 if the school is not found
                    return NotFound(new { message = $"School with ID {schoolId} not found." });
                }

                // Return 200 OK with the school details in the response
                return Ok(schoolDto);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument exceptions (e.g., school not found)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving school details.", error = ex.Message });
            }
        }

        [HttpGet("withcity")]
        public async Task<IActionResult> GetSchoolsWithCity()
        {
            try
            {
                var schools = await _schoolService.GetSchoolsWithCityAsync();

                if (schools == null || !schools.Any())
                {
                    return NotFound(new { message = "No schools found." });
                }

                return Ok(schools);
            }
            catch (ApplicationException ex)
            {
                // Log the exception here (if necessary)
                return StatusCode(500, new { message = "An error occurred while retrieving schools.", details = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("UpdateSchool/{id}")]
        public async Task<IActionResult> UpdateSchool(int id, [FromBody] UpdateSchoolDTO updateSchoolDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to the School entity
            var schoolUpdate = updateSchoolDto.ToUpdateSchool();

            // Call service method to update the school
            var updatedSchool = await _schoolService.UpdateSchoolAsync(id, schoolUpdate);

            if (updatedSchool == null)
            {
                return NotFound(); // Returns 404 if the school does not exist
            }

            return StatusCode(200, new { message = "Updated successfully", status = 200 });
        }


        [HttpGet("GetOpenSchoolsWithAuthorityList")]
        public async Task<ActionResult<List<SchoolWithAuthorityDTO>>> GetOpenSchoolsWithAuthorityList()
        {
            try
            {
                // Call the service method to get the school details
                var schoolsList = await _schoolService.GetSchoolWithAuthorityListAsync();

                if (!schoolsList.Any())
                {
                   // return Ok(new List<SchoolWithAuthorityDTO>());
                }

                // Return 200 OK with the school details in the response
                var schoolDtoList = schoolsList.Select(s => s.ToGetSchoolWithAuthorityDTO()).ToList();
                return Ok(schoolDtoList);
            }

            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving school details.", error = ex.Message });
            }
        }
    }
}
