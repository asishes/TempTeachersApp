using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.PromotionDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferRequestController : ControllerBase
    {
        private readonly TeachersAppDbcontext _context;
        private readonly ITransferRequestService _transferRequestService;
        private readonly ILogger<TransferRequestController> _logger;

        public TransferRequestController ( ITransferRequestService transferRequestService , ILogger<TransferRequestController> logger,TeachersAppDbcontext context)
        {
            _transferRequestService = transferRequestService;
            _logger = logger;
            _context = context;
        }
        [Authorize]
        [HttpPost("CreateTransferRequest")]
        public async Task<IActionResult> CreateTransferRequest([FromBody] CreateTransferRequestDTO createTransferRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Convert DTO to entity model
                var transferRequest = createTransferRequest.ToCreateTransferRequest();

                // Inline check for an existing active transfer request for the same employee
                var existingRequest = await _context.TransferRequests
                    .AnyAsync(tr => tr.EmployeeID == transferRequest.EmployeeID && tr.Status.StatusText == "Pending");

                if (existingRequest)
                {
                    // Return a conflict response if a transfer request already exists
                    return Conflict(new { message = "An active transfer request already exists for this employee.", status = 409 });
                }
                // Retrieve the current school for the employee
                var currentSchool = await _context.Employees
                    .Where(e => e.EmployeeID == transferRequest.EmployeeID)
                    .Select(e => e.SchoolID)
                    .FirstOrDefaultAsync();

                // Check if the destination school is the same as the current school
                if (currentSchool == transferRequest.ToSchoolIDApproved)
                {
                    // Return a conflict response if the destination school is the same as the current school
                    return Conflict(new { message = "Cannot transfer to the same school.", status = 410 });
                }


                // Convert DTO to TransferRequest entity using the mapper
                var convertedTransfer = createTransferRequest.ToCreateTransferRequest();

                // Create the transfer request via the service
                var newTransferRequest = await _transferRequestService.CreateTransferRequestAsync(convertedTransfer);

                if (newTransferRequest == null)
                {
                    _logger.LogWarning("Failed to create Transfer request.");
                    return NotFound(new { message = "Failed to create Transfer Request." });
                }

                // Return a success message with created transfer request details
                return Ok(new
                {
                    message = "Transfer request created successfully",
                    status = 200,
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while creating the transfer request.");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        [HttpGet("GetAllTeacherTransferRequests")]
        public async Task<IActionResult> GetAllTeacherTransferRequests()
        {
            var transferRequests = await _transferRequestService.GetAllTeacherTransferRequestAsync();

            if (transferRequests == null || !transferRequests.Any())
            {
                return NotFound("No transfer requests found.");
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var transferRequestDTOs = transferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(transferRequestDTOs);
        }
        [Authorize]
        [HttpGet("GetAllNonTeacherTransferRequests")]
        public async Task<IActionResult> GetAllNonTeacherTransferRequests()
        {
            var transferRequests = await _transferRequestService.GetAllNonTeacherTransferRequestAsync();

            if (transferRequests == null || !transferRequests.Any())
            {
                return NotFound("No transfer requests found.");
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var transferRequestDTOs = transferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(transferRequestDTOs);
        }
        [Authorize]
        [HttpGet("GetAllTransferedNonTeacher")]
        public async Task<IActionResult> GetAllTransferedNonTeacher()
        {
            var transferRequests = await _transferRequestService.GetAllTransferedNonTeacherAsync();

            if (transferRequests == null || !transferRequests.Any())
            {
                return NotFound("No transfer requests found.");
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var transferRequestDTOs = transferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(transferRequestDTOs);
        }



        [Authorize]
        [HttpGet("GetAllTransferedTeacher")]
        public async Task<IActionResult> GetAllTransferedTeacher()
        {
            var transferRequests = await _transferRequestService.GetAllTransferedTeacherAsync();

            if (transferRequests == null || !transferRequests.Any())
            {
                return NotFound("No transfer requests found.");
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var transferRequestDTOs = transferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(transferRequestDTOs);
        }

        [Authorize]
        [HttpPatch("ApproveTransferRequest/{id}")]
        public async Task<IActionResult> ApproveTransferRequest(int id, [FromBody] ApproveTransferRequestDTO approveTransferRequestDto)
        {
            try
            {
                var approvedTransfer = await _transferRequestService.ApproveTransferAsync(id, approveTransferRequestDto);

                if (approvedTransfer == null)
                {
                    return NotFound(new { message = "Transfer request not found or already approved", status = 404 });
                }

                return Ok(new { message = "Transfer request approved successfully", status = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the transfer request.");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        [HttpPut("RejectTransferRequest/{id}")]
        public async Task<IActionResult> RejectTransfer(int id, [FromBody] RejectTransferRequestDTO rejectTransferDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Convert DTO to the School entity
                var rejectTransferRequest = rejectTransferDTO.ToRejectTransfer();

                // Call service method to update the school
                var rejectTransfer = await _transferRequestService.RejectTransferAsync(id, rejectTransferRequest);

                if (rejectTransfer == null)
                {
                    return NotFound(); // Returns 404 if the school does not exist
                }

                return StatusCode(200, new { message = "Rejected successfully", status = 200 });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message, status = 400 });
            }
        }
        [Authorize]
        [HttpGet("TeacherTransferCompletedfilter")]
        public async Task<IActionResult> TeacherTransferCompletedfilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID,
            [FromQuery] DateTime? fromTransferDate,
            [FromQuery] DateTime? toTransferDate,
            [FromQuery] DateTime? fromWithEffectDate,
            [FromQuery] DateTime? toWithEffectDate)
        {
            try
            {
                var transfer = await _transferRequestService.FilterTeacherTransferCompletedListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromTransferDate,
                    toTransferDate,
                    fromWithEffectDate,
                    toWithEffectDate);

                if (!transfer.Any())
                {
                    return Ok(new List<GetTransferRequestDTO>());

                }

                // Convert to DTOs
                var transferDTOs = transfer
                    .Select(p => p.ToGetTransferRequestDTO())
                    .ToList();

                return Ok(transferDTOs);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("NonTeacherTransferCompletedfilter")]
        public async Task<IActionResult> NonTeacherTransferCompletedfilter(
           [FromQuery] int? schoolID,
           [FromQuery] int? designationID,
           [FromQuery] string? uniqueID,
           [FromQuery] DateTime? fromTransferDate,
           [FromQuery] DateTime? toTransferDate,
           [FromQuery] DateTime? fromWithEffectDate,
           [FromQuery] DateTime? toWithEffectDate)
        {
            try
            {
                var transfer = await _transferRequestService.FilterNonTeacherTransferCompletedListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromTransferDate,
                    toTransferDate,
                    fromWithEffectDate,
                    toWithEffectDate);

                if (!transfer.Any())
                {
                    return Ok(new List<GetTransferRequestDTO>());

                }

                // Convert to DTOs
                var transferDTOs = transfer
                    .Select(p => p.ToGetTransferRequestDTO())
                    .ToList();

                return Ok(transferDTOs);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("TeacherTransferRequestfilter")]
        public async Task<IActionResult> TeacherTransferRequestListFilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID,
            [FromQuery] DateTime? fromTransferDate,
            [FromQuery] DateTime? toTransferDate
            )
        {
            try
            {
                var transfer = await _transferRequestService.FilterTeacherTransferRequestListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromTransferDate,
                    toTransferDate);

                if (!transfer.Any())
                {
                    return Ok(new List<GetTransferRequestDTO>());

                }

                // Convert to DTOs
                var transferDTOs = transfer
                    .Select(p => p.ToGetTransferRequestDTO())
                    .ToList();

                return Ok(transferDTOs);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("NonTeacherTransferRequestfilter")]
        public async Task<IActionResult> NonTeacherTransferRequestListFilter(
            [FromQuery] int? schoolID,
            [FromQuery] int? designationID,
            [FromQuery] string? uniqueID,
            [FromQuery] DateTime? fromTransferDate,
            [FromQuery] DateTime? toTransferDate
            )
        {
            try
            {
                var transfer = await _transferRequestService.FilterNonTeacherTransferRequestListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromTransferDate,
                    toTransferDate);

                if (!transfer.Any())
                {
                    return Ok(new List<GetTransferRequestDTO>());

                }

                // Convert to DTOs
                var transferDTOs = transfer
                    .Select(p => p.ToGetTransferRequestDTO())
                    .ToList();

                return Ok(transferDTOs);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("GetTransferRequestsByEmployeeID/{employeeID}")]
        public async Task<IActionResult> GetTransferRequestsByEmployeeID(int employeeID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transferRequests = await _transferRequestService.GetTransferRequestByEmployeeIdAsync(employeeID);

            if (transferRequests == null || !transferRequests.Any())
            {
                return Ok(new { StatusCode = 204, Message = "No employee found" });
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var transferRequestDTOs = transferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(transferRequestDTOs);
        }

        [HttpGet("GetTeachersTransferRequestsBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetTeachersTransferRequestsBySchoolID(int schoolID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TrnsferRequests = await _transferRequestService.GetTeachersTransferRequestsBySchoolIDAsync(schoolID);

            if (TrnsferRequests == null)
            {
                return Ok(new { StatusCode = 204, Message = "No School found" });
            }

            if (!TrnsferRequests.Any())
            {
                return Ok(new List<GetTransferRequestDTO>());
            }

            var TransferDTOs = TrnsferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(TransferDTOs);
        }

        [HttpGet("GetNonTeachersTransferRequestsBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetNonTeachersTransferRequestsBySchoolID(int schoolID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TrnsferRequests = await _transferRequestService.GetNonTeachersTransferRequestsBySchoolIDAsync(schoolID);

            if (TrnsferRequests == null)
            {
                return Ok(new { StatusCode = 204, Message = "No School found" });
            }

            if (!TrnsferRequests.Any())
            {
                return Ok(new List<GetTransferRequestDTO>());
            }


            var TransferDTOs = TrnsferRequests
                .Select(tr => tr.ToGetTransferRequestDTO())
                .ToList();

            return Ok(TransferDTOs);
        }
    }
}
