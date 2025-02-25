using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileDetailsController : ControllerBase
    {
        private readonly IPersonalDetailsService _personalDetailsService;

        public ProfileDetailsController(IPersonalDetailsService personalDetailsService)
        {
           _personalDetailsService = personalDetailsService;
        }

        [HttpGet("GetAllSubjects")]
        public async Task<ActionResult<List<GetSubjectDTO>>> GetAllSubjectsAsync()
        {
            try
            {
                var result = await _personalDetailsService.GetAllSubjectsAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all subjects.");
            }

        }


        [HttpGet("GetAllEducationTypes")]
        public async Task<ActionResult<List<GetSubjectDTO>>> GetAllEducationTypesAsync()
        {
            try
            {
                var result = await _personalDetailsService.GetAllEducationTypeAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all educationtypes.");
            }
        }

        [HttpGet("GetAllPhotos")]
        public async Task<ActionResult<List<GetPhotoDTO>>> GetAllPhotosAsync()
        {
            try
            {
                var result = await _personalDetailsService.GetAllPhotosAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all photos.");
            }
        }



        [HttpGet("GetAllStatuses")]
        public async Task<ActionResult<List<GetStatusDTO>>> GetAllStatusesAsync()
        {
            try
            {
                var result = await _personalDetailsService.GetAllStatusesAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all statuses.");
            }
        }

        [HttpGet("GetAllSchools")]
        public async Task<ActionResult<List<GetSchoolDTO>>> GetAllSchoolsAsync()
        {
            try
            {
                var result = await _personalDetailsService.GetAllSchoolsAsync();
                return Ok(result);
            }
            catch (Exception)
            { 
                return BadRequest("An error occurred while retrieving all schools.");
            }
        }

        [HttpGet("GetAllReligions")]
        public async Task<ActionResult<List<GetReligionDTO>>> GetAllEmployeeReligionsAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllEmployeeReligionsAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all religions.");
            }
        }

        

        [HttpGet("GetAllMaritalStatuses")]
        public async Task<ActionResult<List<GetMaritalStatusDTO>>> GetAllEmployeeMaritalStatusesAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllEmployeeMaritalStatusesAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all maritalstatuses.");
            }
        }

        [HttpGet("GetAllGenders")]
        public async Task<ActionResult<List<GetGenderDTO>>> GetAllEmployeeGendersAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllEmployeeGendersAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all genders.");
            }
        }

        [HttpGet("GetAllEmployeeTypes")]
        public async Task<ActionResult<List<GetEmployeeTypeDTO>>> GetAllEmployeeTypesAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllEmployeeTypesAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all employeetypes.");
            }
        }

        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<List<GetEmployeeDTO>>> GetAllEmployeesAsync()
        {
           
            try
            {
                var result = await _personalDetailsService.GetAllEmployeesAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all employees.");
            }
        }

        [HttpGet("GetAllEmployeeCategories")]
        public async Task<ActionResult<List<GetEmployeeCategoryDTO>>> GetAllEmployeeCategoriesAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllEmployeeCategoriesAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all employeecategories.");
            }
        }

        [HttpGet("GetAllDistricts")]
        public async Task<ActionResult<List<GetDistrictDTO>>> GetAllDistrictsAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllDistrictsAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all districts.");
            }
        }

        [HttpGet("GetAllDesignations")]
        public async Task<ActionResult<List<GetDesignationDTO>>> GetAllDesignationsAsync()
        {
           
            try
            {
                var result = await _personalDetailsService.GetAllDesignationsAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all designations.");
            }
        }

        [HttpGet("GetAllCourses")]
        public async Task<ActionResult<List<GetCourseDTO>>> GetAllCoursesAsync()
        {
           
            try
            {
                var result = await _personalDetailsService.GetAllCoursesAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all cources.");
            }
        }

        [HttpGet("GetAllCities")]
        public async Task<ActionResult<List<GetCityDTO>>> GetAllCitiesAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllCitiesAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all cities.");
            }
        }

        [HttpGet("GetAllCasteCategories")]
        public async Task<ActionResult<List<GetCasteCategoryDTO>>> GetAllCasteCategoriesAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllCasteCategoriesAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all castecategories.");
            }
        }

        [HttpGet("GetAllBloodGroups")]
        public async Task<ActionResult<List<GetBloodGroupDTO>>> GetAllBloodGroupsAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllBloodGroupsAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all bloodgroups.");
            }
        }

        [HttpGet("GetAllApprovalTypes")]
        public async Task<ActionResult<List<GetApprovalTypeDTO>>> GetAllApprovalTypesAsync()
        {
            
            try
            {
                var result = await _personalDetailsService.GetAllApprovalTypesAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving all approvaltypes.");
            }
        }
    }
}
