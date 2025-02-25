using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.NonTeacherDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Filters;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonTeacherController : ControllerBase
    {
        TeachersAppDbcontext _context;
        private readonly INonTeacherInterface _nonTeacherService;
        private readonly ILogger<NonTeacherController> _logger;
        private IPersonalDetailsService _personalDetailsService;

        public NonTeacherController(INonTeacherInterface nonTeacherInterface,IPersonalDetailsService personalDetailsService, ILogger<NonTeacherController> logger,TeachersAppDbcontext context)
        {
            _nonTeacherService = nonTeacherInterface;
            _logger = logger;
            _personalDetailsService = personalDetailsService;
            _context=context;
        }


        [HttpGet("GetAllActiveNonTeachersCount")]
        public async Task<ActionResult<int>> GetTotalActiveNonTeachersCount()
        {
            try
            {
                var count = await _nonTeacherService.GetTotalActiveNonTeachersCountAsync();
                return Ok(count);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving the active employee count.");
            }


        }


        [Authorize]
        [HttpGet("GetAllActiveNonTeachersList")]
            public async Task<IActionResult> GetNonTeacherListData()
            {
                try
                {

                    var result = await _nonTeacherService.GetNonTeachersListAsync();
                    if (result == null || !result.Any())
                    {
                        return NotFound(new { message = "No Employee found." });
                    }

                    return Ok(result);
                }
                catch (ApplicationException ex)
                {
                    return StatusCode(500, new { message = "An error occurred while retrieving Employees.", details = ex.Message });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
                }


            }


        [HttpGet("GetNonTeachersFilterList")]
        public async Task<IActionResult> GetNonTeacherDisplayFilterData(
            [FromQuery] int? subjectId = null,
            [FromQuery] int? retiringInMonths = null,
            [FromQuery] int? schoolId = null,
            [FromQuery] string? uniqueId = null,
            [FromQuery] bool? includeDocumentsWithError = null,
            [FromQuery] int? minExperienceYear = null,
            [FromQuery] int? maxExperienceYear = null)
        {
            try
            {
                var result = await _nonTeacherService.GetFilterListNonTeachersDataAsync(
                    subjectId,
                    retiringInMonths,
                    schoolId,
                    uniqueId,
                    includeDocumentsWithError,
                    minExperienceYear,
                    maxExperienceYear);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("GetAllNonTeachersDynamicList")]
        public async Task<IActionResult> GetDynamicNonTeacherListData([FromBody] TeacherDymanicResponceDTO request)
        {
            try
            {
                var employees = await _nonTeacherService.GetDynamicListNonTeachersDataAsync(request.Statuses, request.SchoolID);

                if (employees == null || !employees.Any())
                {
                    return Ok(new List<TeacherListDTO>());
                }

                var result = employees.Select(e => e.GetEmployeeListDTO());
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employees.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("GetNonTeachersFilterDynamicList")]
        public async Task<IActionResult> GetNonTeacherFilterDynamicData(
            [FromBody] NonTeacherDymanicFilterResponceDTO nonTeacherDymanicResponceDTO)
        {
            try
            {

                var employees = await _nonTeacherService.GetFilterDynamicListNonTeachersDataAsync(
                    nonTeacherDymanicResponceDTO.RetiringInMonths,
                    nonTeacherDymanicResponceDTO.SchoolId,
                    nonTeacherDymanicResponceDTO.UniqueId,
                    nonTeacherDymanicResponceDTO.MinExperienceYear,
                    nonTeacherDymanicResponceDTO.MaxExperienceYear,
                    nonTeacherDymanicResponceDTO.Statuses,
                    nonTeacherDymanicResponceDTO.AssignedSchoolId
                );
                if (employees == null || !employees.Any())
                {
                    return Ok(new List<TeacherListDTO>());
                }

                return Ok(employees?.Select(e => e.GetEmployeeListDTO()).ToList() ?? new List<TeacherListDTO>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpPost("AddEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateNonTeacherDTO addEmployeeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var availableCourses = await _personalDetailsService.GetAllCoursesAsync();
            var convertedEmployee = addEmployeeDTO.ToAddNonTeacherEmployee(availableCourses);

            try
            {
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.PEN) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PEN == convertedEmployee.PersonalDetails.PEN))
                {
                    _logger.LogWarning("Duplicate entry found for PEN: {PEN}", convertedEmployee.PersonalDetails.PEN);
                    return Conflict(new { message = "Duplicate entry found for the provided PEN." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.Email) &&
                    await _context.Employees.AnyAsync(e => e.Email == convertedEmployee.Email))
                {
                    _logger.LogWarning("Duplicate entry found for Email: {Email}", convertedEmployee.Email);
                    return Conflict(new { message = "Duplicate entry found for the provided Email." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.Phone) &&
                    await _context.Employees.AnyAsync(e => e.Phone == convertedEmployee.Phone))
                {
                    _logger.LogWarning("Duplicate entry found for Phone: {Phone}", convertedEmployee.Phone);
                    return Conflict(new { message = "Duplicate entry found for the provided Phone number." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.AadhaarID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.AadhaarID == convertedEmployee.PersonalDetails.AadhaarID))
                {
                    _logger.LogWarning("Duplicate entry found for AadhaarID: {AadhaarID}", convertedEmployee.PersonalDetails.AadhaarID);
                    return Conflict(new { message = "Duplicate entry found for the provided Aadhaar ID." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.PanID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PanID == convertedEmployee.PersonalDetails.PanID))
                {
                    _logger.LogWarning("Duplicate entry found for PanID: {PanID}", convertedEmployee.PersonalDetails.PanID);
                    return Conflict(new { message = "Duplicate entry found for the provided PAN ID." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.VoterID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.VoterID == convertedEmployee.PersonalDetails.VoterID))
                {
                    _logger.LogWarning("Duplicate entry found for VoterID: {VoterID}", convertedEmployee.PersonalDetails.VoterID);
                    return Conflict(new { message = "Duplicate entry found for the provided Voter ID." });
                }

                var newEmployee = await _nonTeacherService.CreateNonTeacherAsync(convertedEmployee, addEmployeeDTO);

                if (newEmployee == null)
                {
                    _logger.LogWarning("Failed to create employee.");
                    return NotFound(new { message = "Failed to create employee." });
                }
                return StatusCode(200, new { message = "Employee created successfully", status = 200 });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Position is not valid"))
                {
                    return StatusCode(300, new { message = ex.Message, status = 400 });
                }

                if (ex.Message.Contains("Only 'Office Staff' designation is allowed"))
                {
                    return StatusCode(422, new { message = ex.Message, status = 422 });
                }

                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employee.");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpPut("update-employee/{employeeId:int}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int employeeId, [FromBody] UpdateNonTeacherDTO updateEmployee)
        {
            _logger.LogInformation("Entered UpdateEmployee with employeeId: {employeeId}", employeeId);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            var employeeModel = updateEmployee.ToUpdateNonTeacherEmployee();
            try
            {
                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.PEN) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PEN == employeeModel.PersonalDetails.PEN && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate entry found for PEN: {PEN}", employeeModel.PersonalDetails.PEN);
                    return Conflict(new { message = "Duplicate entry found for the provided PEN." });
                }
                if (!string.IsNullOrEmpty(employeeModel.Email) &&
                    await _context.Employees.AnyAsync(e => e.Email == employeeModel.Email && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate entry found for Email: {Email}", employeeModel.Email);
                    return Conflict(new { message = "Duplicate entry found for the provided Email." });
                }
                if (!string.IsNullOrEmpty(employeeModel.Phone) &&
                    await _context.Employees.AnyAsync(e => e.Phone == employeeModel.Phone && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate entry found for Phone: {Phone}", employeeModel.Phone);
                    return Conflict(new { message = "Duplicate entry found for the provided Phone number." });
                }
 
                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.AadhaarID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.AadhaarID == employeeModel.PersonalDetails.AadhaarID && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate entry found for AadhaarID: {AadhaarID}", employeeModel.PersonalDetails.AadhaarID);
                    return Conflict(new { message = "Duplicate entry found for the provided Aadhaar ID." });
                }
                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.PanID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PanID == employeeModel.PersonalDetails.PanID && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate entry found for PanID: {PanID}", employeeModel.PersonalDetails.PanID);
                    return Conflict(new { message = "Duplicate entry found for the provided PAN ID." });
                }
                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.VoterID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.VoterID == employeeModel.PersonalDetails.VoterID && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate entry found for VoterID: {VoterID}", employeeModel.PersonalDetails.VoterID);
                    return Conflict(new { message = "Duplicate entry found for the provided Voter ID." });
                }
                
                // Call the service to update the employee
                var updatedEmployee = await _nonTeacherService.UpdateNonTeacherAsync(employeeId, employeeModel).ConfigureAwait(false);

                // Log after service call
                _logger.LogInformation("Service call completed for employeeId: {employeeId}", employeeId);

                // Check if the update was successful
                if (updatedEmployee == null)
                {
                    _logger.LogWarning("No employee found with ID: {employeeId}", employeeId);
                    return NotFound(new { message = "No data found" });
                }

                // Return a success message
                return StatusCode(200, new { message = "Updated successfully", status = 200 });
            }

            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while updating the employee with ID {employeeId}", employeeId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("NonTeacherHomepage/{id}")]
        public async Task<ActionResult<ToGetEmployeeDTO>> GetNonTeacherEmployeeHomePage(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _nonTeacherService.GetNonTeacherHomePageAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = "No data found" });
            }
            var studentDTO = employee.ToGetNonTeacherEmployee();

            return Ok(studentDTO);
        }

        [HttpGet("GetAllRetiredNonTeacher")]
        public async Task<IActionResult> GetAllRetiredNonTeacher()
        {
            var retiredNonTeachers = await _nonTeacherService.GetAllRetiredNonTeacherAsync();


            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var employeeDTOs = retiredNonTeachers
                .Select(tr => tr.GetNonTeacherEmployeeListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }


        [HttpGet("GetAllNonTeacherOnLeave")]
        public async Task<IActionResult> GetAllNonTeacherOnLeave()
        {
            var retiredNonTeachers = await _nonTeacherService.GetAllNonTeacherOnLeaveAsync();


            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var employeeDTOs = retiredNonTeachers
                .Select(tr => tr.GetEmployeeLeaveDTO())
                .ToList();

            return Ok(employeeDTOs);
        }

        [HttpGet("GetNonTeachersBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetNonTeachersBySchoolID(int schoolID)
        {
            try
            {
                var teachers = await _nonTeacherService.GetNonTeachersBySchoolIDAsync(schoolID);

                // Map each Employee to TeacherListDTO using the mapper method
                var employeeDTOs = teachers.Select(tr => tr.ToGetStaffListDTO()).ToList();

                return Ok(employeeDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet("GetAllApprovedNonTeacher")]
        public async Task<IActionResult> GetAllApprovedNonTeacher()
        {
            var approvedNonTeachers = await _nonTeacherService.GetApprovedNonTeacherAsync();

            var employeeDTOs = approvedNonTeachers
                .Select(tr => tr.GetEmployeeListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }


        [HttpGet("GetAllNonApprovedNonTeacher")]
        public async Task<IActionResult> GetAllNonApprovedNonTeacher()
        {
            var nonApprovedNonTeachers = await _nonTeacherService.GetNonApprovedNonTeacherAsync();


           
            var employeeDTOs = nonApprovedNonTeachers
                .Select(tr => tr.GetEmployeeListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }

        [HttpGet("GetNonTeachersStatusCount")]
        public async Task<IActionResult> GetNonTeachersStatusCount()
        {
            var result = await _nonTeacherService.GetTotalNonTeachersStatusCountAsync();
            return Ok(result);
        }

        [Authorize]
        [CustomRole("Head Master")]
        [HttpGet("GetOnLeaveNonTeachersBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetOnLeaveNonTeachersBySchoolID(int schoolID)
        {
            try
            {
                var onLeaveTeachers = await _nonTeacherService.GetOnLeaveNonTeachersBySchoolIDAsync(schoolID);

                if (onLeaveTeachers == null)
                {
                    return Ok(new
                    {
                        message = "SchoolID Not Found.",
                        status = 204
                    });
                }

                if (!onLeaveTeachers.Any())
                {
                    return Ok(new List<TeacherListDTO>());
                }

                var employeeDTOs = onLeaveTeachers.Select(tr => tr.GetEmployeeListDTO()).ToList();

                return Ok(employeeDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Error retrieving data.",
                    error = ex.Message
                });
            }
        }
    }
}

