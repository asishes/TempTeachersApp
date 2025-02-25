using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Filters;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly TeachersAppDbcontext _context;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILogger<LeaveRequestController> _logger;

        public LeaveRequestController (ILeaveRequestService leaveRequestService, TeachersAppDbcontext context, ILogger<LeaveRequestController> logger)
        {
            _leaveRequestService = leaveRequestService;
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("CreateLeaveRequest")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDTO createLeaveRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var leaveRequest = createLeaveRequest.ToCreateLeaveRequest();

                // Get employee status
                var employee = await _context.Employees
                    .Where(e => e.EmployeeID == leaveRequest.EmployeeID)
                    .Select(e => new { e.StatusID })
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found.", status = 404 });
                }

                // Check if employee status is "Leave" (return status 309)
                var leaveStatusId = await _context.Statuses
                    .Where(s => s.StatusText == "Leave" && s.StatusType == "Employee")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();

                if (employee.StatusID == leaveStatusId)
                {
                    return StatusCode(309, new { message = "Employee is on leave.", status = 309 });
                }

                // Ensure employee is active
                var activeStatusId = await _context.Statuses
                    .Where(s => s.StatusText == "Active" && s.StatusType == "Employee")
                    .Select(s => s.StatusID)
                    .FirstOrDefaultAsync();

                // Check if an active leave request already exists
                var existingRequest = await _context.LeaveRequests
                    .AnyAsync(tr => tr.EmployeeID == leaveRequest.EmployeeID &&
                        (tr.Status.StatusText == "Pending" || tr.Status.StatusText == "HMApproved"));

                if (existingRequest)
                {
                    return Conflict(new { message = "An active Leave request already exists for this employee.", status = 409 });
                }

                var newLeaveRequest = await _leaveRequestService.CreateLeaveRequestAsync(leaveRequest);

                if (newLeaveRequest == null)
                {
                    _logger.LogWarning("Failed to create Leave request.");
                    return NotFound(new { message = "Failed to create Leave Request." });
                }

                return Ok(new
                {
                    message = "Leave request created successfully",
                    status = 200,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Leave request.");
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize]
        [HttpGet("GetAllTeacherLeaveRequests")]
        public async Task<IActionResult> GetAllTeacherLeaveRequests()
        {
            var leaveRequests = await _leaveRequestService.GetAllTeacherLeaveRequestAsync();

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return NotFound("No Leave requests found.");
            }

            var leaveRequestDTOs = leaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveRequestDTOs);
        }


        [Authorize]
        [HttpGet("GetAllNonTeacherLeaveRequests")]
        public async Task<IActionResult> GetAllNonTeacherLeaveRequests()
        {
            var leaveRequests = await _leaveRequestService.GetAllNonTeacherLeaveRequestAsync();

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return NotFound("No Leave requests found.");
            }

            var leaveRequestDTOs = leaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveRequestDTOs);
        }

        [Authorize]
        [HttpGet("GetAllLeaveTeacherLeave")]
        public async Task<IActionResult> GetAllLeaveTeacher()
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveTeacherAsync();

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return NotFound("No Leave requests found.");
            }

            var leaveRequestDTOs = leaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveRequestDTOs);
        }

        
        [Authorize]
        [HttpGet("GetAllLeaveNonTeacher")]
        public async Task<IActionResult> GetAllLeaveNonTeacher()
        {
            var leaveRequests = await _leaveRequestService.GetAllLeaveNonTeacherAsync();

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return NotFound("No Leave requests found.");
            }

            var leaveRequestDTOs = leaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveRequestDTOs);
        }

       // [Authorize]
        //[CustomRole("Head Master")]
        [HttpPut("ApproveLeaveByHeadMaster/{Id}")]
        public async Task<IActionResult> ApproveLeaveByHeadMaster(int Id)
        {
            try
            {
                // Call the service method to approve the employee by changing their status to HMApproved
                var approvedEmployee = await _leaveRequestService.ApproveLeaveRequestByHeadMasterAsync(Id);

                if (approvedEmployee == null)
                {
                    return NotFound(new { message = "Employee not found or status not found." });
                }

                return Ok(new
                {
                    message = "Leave status updated to HMApproved.",
                    status = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { details = ex.Message }); // Handle errors with an empty message
            }
        }


        [Authorize]
        [HttpPatch("ApproveLeaveRequest/{id}")]
        public async Task<IActionResult> ApproveLeaveRequest(int id, [FromBody] ApproveLeaveRequestDTO approveLeaveRequestDto)
        {
            try
            {
                var approvedTransfer = await _leaveRequestService.ApproveLeaveRequestAsync(id, approveLeaveRequestDto);

                if (approvedTransfer == null)
                {
                    return NotFound(new { message = "Leave request not found or already approved", status = 404 });
                }

                return Ok(new { message = "Leave request approved successfully", status = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the Leave request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut("RejectLeaveRequest/{id}")]
        public async Task<IActionResult> RejectTransfer(int id, [FromBody] RejectLeaveRequestDTO rejectLeaveRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var rejectLeaveRequest = rejectLeaveRequestDTO.ToRejectLeaveRequest();

                var rejectLeave = await _leaveRequestService.RejectLeaveRequestAsync(id, rejectLeaveRequest);

                if (rejectLeave == null)
                {
                    return NotFound();
                }

                return StatusCode(200, new { message = "Leave Rejected successfully", status = 200 });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message, status = 400 });
            }
        }


        [HttpGet("NonTeacherLeaveRequestedfilter")]
        public async Task<IActionResult> NonTeacherLeaveRequestedfilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID)
        {
            try
            {
                var leave = await _leaveRequestService.FilterNonTeacherLeaveRequestedListAsync(
                    schoolID,
                    designationID,
                    uniqueID);

                if (!leave.Any())
                {
                    return Ok(new List<GetLeaveRequestDTO>());

                }

                var leaveDTOs = leave
                    .Select(p => p.ToGetLeaveRequestDTO())
                    .ToList();

                return Ok(leaveDTOs);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("TeacherLeaveRequestedfilter")]
        public async Task<IActionResult> TeacherLeaveRequestedfilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID)
        {
            try
            {
                var leave = await _leaveRequestService.FilterTeacherLeaveRequestedListAsync(
                    schoolID,
                    designationID,
                    uniqueID);

                if (!leave.Any())
                {
                    return Ok(new List<GetLeaveRequestDTO>());

                }

                var leaveDTOs = leave
                    .Select(p => p.ToGetLeaveRequestDTO())
                    .ToList();

                return Ok(leaveDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("NonTeacherLeaveCompletedfilter")]
        public async Task<IActionResult> NonTeacherPLeaveCompletedfilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID)
        {
            try
            {
                var leave = await _leaveRequestService.FilterNonTeacherLeaveCompletedListAsync(
                    schoolID,
                    designationID,
                    uniqueID);

                if (!leave.Any())
                {
                    return Ok(new List<GetLeaveRequestDTO>());

                }

                var leaveDTOs = leave
                    .Select(p => p.ToGetLeaveRequestDTO())
                    .ToList();

                return Ok(leaveDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("TeacherLeaveCompletedfilter")]
        public async Task<IActionResult> TeacherLeaveCompletedfilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID)
        {
            try
            {
                var leave = await _leaveRequestService.FilterTeacherLeaveCompletedListAsync(
                    schoolID,
                    designationID,
                    uniqueID);

                if (!leave.Any())
                {
                    return Ok(new List<GetLeaveRequestDTO>());

                }

                var leaveDTOs = leave
                    .Select(p => p.ToGetLeaveRequestDTO())
                    .ToList();

                return Ok(leaveDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("GetLeaveRequestByEmployeeId/{employeeID}")]
        public async Task<IActionResult> GetLeaveRequestByEmployeeId(int employeeID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var leaveRequests = await _leaveRequestService.GetLeaveRequestsByEmployeeIdAsync(employeeID);

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return Ok(new { StatusCode = 204, Message = "No employee found" });
            }
            var leaveRequestDTOs = leaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveRequestDTOs);
        }

        [HttpGet("GetTeachersLeaveRequestsBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetTeachersLeaveRequestsBySchoolID(int schoolID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var LeaveRequests = await _leaveRequestService.GetTeachersLeaveRequestsBySchoolIDAsync(schoolID);

            if (LeaveRequests == null)
            {
                return Ok(new { StatusCode = 204, Message = "No School found" });
            }

            if (!LeaveRequests.Any())
            {
                return Ok(new List<GetLeaveRequestDTO>());
            }


            var leaveDTOs = LeaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveDTOs);
        }

        [HttpGet("GetNonTeachersLeaveRequestsBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetNonTeachersLeaveRequestsBySchoolID(int schoolID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var LeaveRequests = await _leaveRequestService.GetNonTeachersLeaveRequestsBySchoolIDAsync(schoolID);

            if (LeaveRequests == null)
            {
                return Ok(new { StatusCode = 204, Message = "No School found" });
            }

            if (!LeaveRequests.Any())
            {
                return Ok(new List<GetLeaveRequestDTO>());
            }


            var leaveDTOs = LeaveRequests
                .Select(tr => tr.ToGetLeaveRequestDTO())
                .ToList();

            return Ok(leaveDTOs);
        }
        [HttpDelete("DeleteLeaveRequests/{id}")]
        public async Task<IActionResult> DeleteLeaveRequests(int id)
        {
            try
            {
                var result = await _leaveRequestService.DeleteLeaveRequestsAsync(id);
                if (!result)
                {
                    return NotFound("No LeaveRequests found.");
                }
            }
            catch (ArgumentException ex)
            {

                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Leave request with the given ID not found."))
                {
                    return StatusCode(400, new { message = ex.Message, status = 400 });
                }
                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Leave request is not in pending status."))
                {
                    return StatusCode(400, new { message = ex.Message, status = 401 });
                }
                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Employee is not in 'Active' or 'Leave' status."))
                {
                    return StatusCode(400, new { message = ex.Message, status = 402 });
                }

                return BadRequest(new { message = ex.Message });
            }
            return Ok(new { StatusCode = 200, Message = " LeaveRequest Deleted" });
        }

        [HttpPatch("EditLeaveRequests/{id}")]
        public async Task<IActionResult> EditLeaveRequests(int id, [FromBody] EditLeaveRequestDTO editLeaveRequestDTO)
        {
            try
            {
                if (editLeaveRequestDTO == null)
                {
                    return BadRequest("Invalid Leave data.");
                }

                var updatedUser = await _leaveRequestService.EditLeaveRequestsAsync(id, editLeaveRequestDTO);

                if (updatedUser == null)
                {
                    return NotFound($"LeaveRequest with ID {id} not found.");
                }
            }
            catch (ArgumentException ex)
            {

                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Leave request with the given ID not found."))
                {
                    return StatusCode(400, new { message = ex.Message, status = 400 });
                }
                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Leave request is not in pending status."))
                {
                    return StatusCode(400, new { message = ex.Message, status = 401 });
                }
                _logger.LogWarning(ex.Message);
                if (ex.Message.Contains("Employee is not in 'Active' or 'Leave' status."))
                {
                    return StatusCode(400, new { message = ex.Message, status = 402 });
                }

                return BadRequest(new { message = ex.Message });
            }

            return StatusCode(200, new { message = "Updated successfully", status = 200 });
        }
    }
}
