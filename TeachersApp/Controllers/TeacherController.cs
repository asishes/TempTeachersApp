using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.Helpers;
using TeachersApp.Entity.ModelDTO.CourseDTO;
using TeachersApp.Entity.ModelDTO.DocumentDTO;
using TeachersApp.Entity.ModelDTO.PhotoDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Filters;
using TeachersApp.Services.FileServices;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly TeachersAppDbcontext _context;
        private readonly ITeacherService _teacherService;
        private readonly IPersonalDetailsService _personalDetailsService;
        private readonly ICourseService _courseService;
        private readonly ILogger<TeacherController> _logger;



        public TeacherController(ITeacherService teacherService,IPersonalDetailsService personalDetailsService, TeachersAppDbcontext context,ICourseService courseService,ILogger<TeacherController> logger)
        {
            _teacherService = teacherService;
            _context = context;
            _personalDetailsService = personalDetailsService;
            _courseService = courseService;
            _logger = logger;

        }


        #region ListTeachersData

        [HttpGet("GetAllActiveTeachersList")]
        public async Task<IActionResult> GetTeacherListData()
        {
            try
            {
                var employees = await _teacherService.GetListTeachersDataAsync();

                if (employees == null || !employees.Any())
                {
                    return NotFound(new { message = "No active employees found." });
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

        #endregion



        #region GetTotalActiveTeachersCount
        [HttpGet("GetAllActiveTeachersCount")]
        public async Task<ActionResult<int>> GetTotalActiveTeachersCount()
        {
            try
            {
                // Fetch teacher data from the teacher service
                var count = await _teacherService.GetTotalActiveTeachersCountAsync();

                // Return a 200 OK response with the data
                return Ok(count);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving the active employee count.");
            }


        }
        #endregion



        #region GetPromotionEligibleTeachersCount
        [HttpGet("GetPromotionEligibleTeachersCount")]
        public async Task<ActionResult<int>> GetPromotionEligibleTeachersCount()
        {
            try
            {

                // Fetch teacher data from the teacher service
                var count = await _teacherService.GetPromotionEligibleTeachersCountAsync();

                // Return a 200 OK response with the data
                return Ok(count);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving the active employee promotion eligible count.");
            }


        }
        #endregion







        #region TeacherPopUp

        [Authorize]
        [HttpGet("GetTeacherPopUp/{teacherId}")]
        public async Task<IActionResult> GetTeacherPopup(int teacherId)
        {

            try
            {
                // Call the service method to get the school details
                var teacherPopup = await _teacherService.GetTeacherPopUpAsync(teacherId);

                if (teacherPopup == null)
                {
                    // Return 404 if the school is not found
                    return NotFound(new { message = $"Employee with ID {teacherId} not found." });
                }

                // Return 200 OK with the school details in the response
                return Ok(teacherPopup);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument exceptions (e.g., school not found)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving Employee details.", error = ex.Message });
            }
        }

        #endregion


        [Authorize]
        #region FilterTeacher
        [HttpGet("GetTeachersFilterList")]
        public async Task<IActionResult> GetTeacherDisplayFilterData(
            [FromQuery] int? subjectId = null,
            [FromQuery] int? retiringInMonths = null,
            [FromQuery] int? schoolId = null,
            [FromQuery] string? uniqueId =null,
            [FromQuery] bool? includeDocumentsWithError = null,
            [FromQuery] int? minExperienceYear = null,
            [FromQuery] int? maxExperienceYear = null)
        {
            try
            {
                var result = await _teacherService.GetFilterListTeachersDataAsync(
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
                // Log exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        [Authorize]
        [HttpPost("GetTeachersFilterDynamicList")]
        public async Task<IActionResult> GetTeacherFilterDynamicData(
       [FromBody] TeacherDymanicFilterResponceDTO teacherDymanicResponceDTO)
        {
            try
            {

                var employees = await _teacherService.GetFilterDynamicListTeachersDataAsync(
                    teacherDymanicResponceDTO.SubjectId,
                    teacherDymanicResponceDTO.RetiringInMonths,
                    teacherDymanicResponceDTO.SchoolId,
                    teacherDymanicResponceDTO.UniqueId,
                    teacherDymanicResponceDTO.MinExperienceYear,
                    teacherDymanicResponceDTO.MaxExperienceYear,
                    teacherDymanicResponceDTO.Statuses,
                    teacherDymanicResponceDTO.AssignedSchoolId
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
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO addEmployeeDTO)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the available courses from the database or service
            var availableCourses = await _personalDetailsService.GetAllCoursesAsync(); // Assuming _courseService is injected

            // Convert the DTO to the Employee entity, passing the available courses
            var convertedEmployee = addEmployeeDTO.ToAddEmployee(availableCourses);
            try
            {
                // Duplicate checks for unique fields
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.PEN) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PEN == convertedEmployee.PersonalDetails.PEN))
                {
                    _logger.LogWarning("Duplicate PEN: {PEN}", convertedEmployee.PersonalDetails.PEN);
                    return Conflict(new { message = "Duplicate  PEN." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.Email) &&
                    await _context.Employees.AnyAsync(e => e.Email == convertedEmployee.Email))
                {
                    _logger.LogWarning("Duplicate  Email: {Email}", convertedEmployee.Email);
                    return Conflict(new { message = "Duplicate Email." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.Phone) &&
                    await _context.Employees.AnyAsync(e => e.Phone == convertedEmployee.Phone))
                {
                    _logger.LogWarning("Duplicate Phone: {Phone}", convertedEmployee.Phone);
                    return Conflict(new { message = "Duplicate Phone number." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.AadhaarID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.AadhaarID == convertedEmployee.PersonalDetails.AadhaarID))
                {
                    _logger.LogWarning("Duplicate  AadhaarID: {AadhaarID}", convertedEmployee.PersonalDetails.AadhaarID);
                    return Conflict(new { message = "Duplicate Aadhaar ID." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.PanID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PanID == convertedEmployee.PersonalDetails.PanID))
                {
                    _logger.LogWarning("Duplicate  PanID: {PanID}", convertedEmployee.PersonalDetails.PanID);
                    return Conflict(new { message = "Duplicate PAN ID." });
                }
                if (!string.IsNullOrEmpty(convertedEmployee.PersonalDetails.VoterID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.VoterID == convertedEmployee.PersonalDetails.VoterID))
                {
                    _logger.LogWarning("Duplicate VoterID: {VoterID}", convertedEmployee.PersonalDetails.VoterID);
                    return Conflict(new { message = "Duplicate Voter ID." });
                }
               

                // Create the employee in the database
                var newEmployee = await _teacherService.CreateEmployeesAsync(convertedEmployee, addEmployeeDTO);

            if (newEmployee is null)
            {
                return Ok(new { message = "School/Designation/PositionId is not available", status = 250 });
            }

                // Return the created employee data
                if (newEmployee == null)
                {
                    _logger.LogWarning("Failed to create employee.");
                    return NotFound(new { message = "Failed to create employee." });
                }

                // Return a success message
                return StatusCode(200, new { message = "Employee created successfully", status = 200 });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while creating the employee.");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut("update-employee/{employeeId:int}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int employeeId, [FromBody] TeacherUpdateDTO updateEmployee)
        {
            _logger.LogInformation("Entered UpdateEmployee with employeeId: {employeeId}", employeeId);

            // Validate the model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // Map the DTO to the Employee entity
            var employeeModel = updateEmployee.ToUpdateEmployee();

            try
            {
                // Duplicate checks for unique fields
                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.PEN) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PEN == employeeModel.PersonalDetails.PEN && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate  PEN: {PEN}", employeeModel.PersonalDetails.PEN);
                    return Conflict(new { message = "Duplicate  PEN." });
                }

                if (!string.IsNullOrEmpty(employeeModel.Email) &&
                    await _context.Employees.AnyAsync(e => e.Email == employeeModel.Email && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate Email: {Email}", employeeModel.Email);
                    return Conflict(new { message = "Duplicate Email." });
                }

                if (!string.IsNullOrEmpty(employeeModel.Phone) &&
                    await _context.Employees.AnyAsync(e => e.Phone == employeeModel.Phone && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate  Phone: {Phone}", employeeModel.Phone);
                    return Conflict(new { message = "Duplicate Phone number." });
                }


                // Additional duplicate checks for other unique fields if needed 
                // e.g., AadhaarID, PanID, etc.
                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.AadhaarID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.AadhaarID == employeeModel.PersonalDetails.AadhaarID && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate  AadhaarID: {AadhaarID}", employeeModel.PersonalDetails.AadhaarID);
                    return Conflict(new { message = "Duplicate Aadhaar ID." });
                }

                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.PanID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.PanID == employeeModel.PersonalDetails.PanID && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate PanID: {PanID}", employeeModel.PersonalDetails.PanID);
                    return Conflict(new { message = "Duplicate PAN ID." });
                }


                if (!string.IsNullOrEmpty(employeeModel.PersonalDetails.VoterID) &&
                    await _context.Employees.AnyAsync(e => e.PersonalDetails.VoterID == employeeModel.PersonalDetails.VoterID && e.EmployeeID != employeeId))
                {
                    _logger.LogWarning("Duplicate VoterID: {VoterID}", employeeModel.PersonalDetails.VoterID);
                    return Conflict(new { message = "Duplicate  Voter ID." });
                }

               
                // Call the service to update the employee
                var updatedEmployee = await _teacherService.UpdateEmployeeAsync(employeeId, employeeModel).ConfigureAwait(false);

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


        [HttpGet("GetCoursesByEducationType/{educationTypeId}")]
        public async Task<IActionResult> GetCoursesByEducationType(int educationTypeId)
        {
            try
            {
                List<GetEmployeeCourseDTO> courses = await _courseService.GetCoursesByEducationTypeIDAsync(educationTypeId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }






        [Authorize]
        [HttpGet("GetEmployeeByUniqueID/{uniqueID}")]
        public async Task<ActionResult<ToGetEmployeeDTO>> GetEmployeeByUniqueID(string uniqueID)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _teacherService.GetEmployeeByUniqueIDAsync(uniqueID);
            if (employee == null)
            {
                return NotFound(new { message = "No data found" });
            }
            var employeeDTO = employee.ToGetEmployee();

            return Ok(employeeDTO);
        }


        [Authorize]
        [HttpGet("get-employee/{EmployeeID:int}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int EmployeeID)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _teacherService.GetEmployeeByIdAsync(EmployeeID);
            if (employee == null)
            {
                return NotFound(new { message = "No data found" });
            }
            var studentDTO = employee.ToGetEmployee();

            return Ok(studentDTO);
        }



        [Authorize]
        [HttpGet("GetAllRetiredTeacher")]
        public async Task<IActionResult> GetAllRetiredTeacher()
        {
            var retiredTeachers = await _teacherService.GetAllRetiredTeacherAsync();


            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var employeeDTOs = retiredTeachers
                .Select(tr => tr.GetEmployeeListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }


        [Authorize]
        [HttpGet("GetAllTeacherOnLeave")]
        public async Task<IActionResult> GetAllTeacherOnLeave()
        {
            var retiredTeachers = await _teacherService.GetAllTeacherOnLeaveAsync();


            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var employeeDTOs = retiredTeachers
                .Select(tr => tr.GetEmployeeLeaveDTO())
                .ToList();

            return Ok(employeeDTOs);
        }


        [Authorize]
        [HttpPost("GetEmployeesBySchoolIDAndSchoolTypeIDAndEmployeeTypeID")]
        public async Task<IActionResult> GetEmployeesBySchoolIDAndSchoolTypeIdAndEmployeeTypeIDAsync(
        [FromBody] AllEmployeeBySIDSTypeIDETypeIDRequestDTO RequestDTO)
        {
            var teachers = await _teacherService.GetEmployeesBySchoolIDAndSchoolTypeIdAndEmployeeTypeIDAsync(
                RequestDTO.SchoolID, RequestDTO.SchoolTypeIDs, RequestDTO.EmployeeTypeIDs);

            if (teachers == null || !teachers.Any())
            {
                return Ok(new List<StaffListDTO>());  // Return an empty list
            }

            var employeeDTOs = teachers
                .Select(tr => tr.ToGetStaffListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }

        [Authorize]
        [HttpGet("GetTeachersBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetTeachersBySchoolID(int schoolID)
        {
            try
            {
                var teachers = await _teacherService.GetTeachersBySchoolIDAsync(schoolID);

                // Map each Employee to TeacherListDTO using the mapper method
                var employeeDTOs = teachers.Select(tr => tr.ToGetStaffListDTO()).ToList();

                return Ok(employeeDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost("process-retirements")]
        public async Task<IActionResult> ProcessRetirements()
        {
            try
            {
                _logger.LogInformation("Processing retirements initiated.");

                await _teacherService.ProcessRetirementsAsync();

                _logger.LogInformation("Retirement processing completed successfully.");
                return Ok(new { Message = "Retirements processed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing retirements: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while processing retirements." });
            }
        }
        [Authorize]
        [HttpGet("GetPromotionEligibleEmployeeList")]
        public async Task<IActionResult> GetPromotionEligibleEmployeeList()
        {
            try
            {
                var employees = await _teacherService.GetPromotionEligibleEmployeeList();

                if (employees == null || !employees.Any())
                {
                    return NotFound(new { message = "No promotion eligible employees found." });
                }

                var result = employees.Select(e => e.ToGetPromotionEligibleEmployeeDTO());
                return Ok(result);
            }
           
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("GetAllApprovedTeacher")]
        public async Task<IActionResult> GetAllApprovedTeacher()
        {
            var approvedTeachers = await _teacherService.GetApprovedTeacherAsync();


            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var employeeDTOs = approvedTeachers
                .Select(tr => tr.GetEmployeeListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }

        [Authorize]
        [HttpGet("GetAllNonApprovedTeacher")]
        public async Task<IActionResult> GetAllNonApprovedTeacher()
        {
            var nonApprovedTeachers = await _teacherService.GetNonApprovedTeacherAsync();

            var employeeDTOs = nonApprovedTeachers
                .Select(tr => tr.GetEmployeeListDTO())
                .ToList();

            return Ok(employeeDTOs);
        }

        [Authorize]
        [HttpGet("GetPromotedDesignationByEmployeeID/{employeeID}")]
        public async Task<IActionResult> GetPromotedDesignationByEmployeeID(int employeeID)
        {
            try
            {
                // Fetch promotion details
                var designation = await _teacherService.GetPromotedDesignationByEmployeeIDAsync(employeeID);

                if (designation == null)
                {
                    return NotFound(new { Message = "No promotion record found for the given employee ID." });
                }

                // Map to DTO
                var designationDTO = designation.ToGetPromotionDesignationDTO();

                return Ok(designationDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("GetAllTeachersDynamicList")]
        public async Task<IActionResult> GetDynamicTeacherListData([FromBody] TeacherDymanicResponceDTO request)
        {
            try
            {
                var employees = await _teacherService.GetDynamicListTeachersDataAsync(request.Statuses, request.SchoolID);

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
        [CustomRole("Head Master")]
        [HttpPut("ApproveEmployeeByHeadMaster/{Id}")]
        public async Task<IActionResult> ApproveEmployeeByHeadMaster(int Id)
        {
            try
            {
                // Call the service method to approve the employee by changing their status to HMApproved
                var approvedEmployee = await _teacherService.ApproveEmployeeByHeadMasterAsync(Id);

                if (approvedEmployee == null)
                {
                    return NotFound(new { message = "Employee not found or status not found." });
                }

                return Ok(new
                {
                    message = "Employee status updated to HMApproved.",
                    status = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {  details = ex.Message }); // Handle errors with an empty message
            }
        }

        [Authorize]
        [CustomRole("Manager","SuperAdmin")]
        [HttpPut("ApproveEmployeeByManager/{Id}")]
        public async Task<IActionResult> ApproveEmployeeByManager(int Id)
        {
            try
            {
                // Call the service method to approve the employee by changing their status to HMApproved
                var approvedEmployee = await _teacherService.ApproveEmployeeByManagerAsync(Id);

                if (approvedEmployee == null)
                {
                    return NotFound(new { message = "Employee not found or status not found." });
                }

                return Ok(new
                {
                    message = "Employee status updated to Active.",
                    status = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {  details = ex.Message }); // Handle errors with an empty message
            }
        }

        [HttpGet("GetTeachersStatusCount")]
        public async Task<IActionResult> GetTeachersStatusCount()
        {
            var result = await _teacherService.GetTotalTeachersStatusCountAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetEmployeeOrderBySeniorityBySchoolID/{schoolId}")]
        public async Task<IActionResult> GetEmployeeOrderBySeniorityBySchoolID(int schoolId)
        {
            var employees = await _teacherService.GetEmployeeOrderByPromotionSeniorityBySchoolIDAsync(schoolId);
            if (employees == null || !employees.Any())
            {
                return Ok(new List<TeacherListDTO>());
            }
            var employeeDTOs = employees
                .Select(tr => tr.ToGetPromotionEmployeeDTO())
                .ToList();

            return Ok(employeeDTOs);
        }

        [Authorize]
        //[CustomRole("Head Master")]
        [HttpGet("GetOnLeaveTeachersBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetOnLeaveTeachersBySchoolID(int schoolID)
        {
            try
            {
                var onLeaveTeachers = await _teacherService.GetOnLeaveTeachersBySchoolIDAsync(schoolID);

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




