using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO.PromotionRelinquishment;
using TeachersApp.Entity.ModelDTO.SchoolDivisionDTO;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly TeachersAppDbcontext _context;
        private readonly ILogger<PromotionController> _logger;
        private readonly IPersonalDetailsService _personalDetailsService;

        public PromotionController(IPromotionService promotionService,TeachersAppDbcontext context, ILogger<PromotionController> logger,IPersonalDetailsService personalDetailsService)
        {
            _promotionService = promotionService;
            _context = context;
            _logger = logger;   
            _personalDetailsService = personalDetailsService;
        }
        [HttpPost("update-seniority")]
        public async Task<IActionResult> UpdateSeniority()
        {
            await _promotionService.UpdatePromotionSeniorityAsync();
            return Ok("Promotion seniority numbers updated successfully.");
        }

        [HttpGet("promotion-list")]
        public async Task<IActionResult> GetPromotionList([FromQuery] int designationId, [FromQuery] int subjectId)
        {
            // Validate input parameters
            if (designationId <= 0 || subjectId <= 0)
            {
                return BadRequest("Invalid DesignationID or SubjectID.");
            }

            try
            {
                // Call the service to get the list of promotion-eligible employees
                var promotionList = await _promotionService.GetEmployeesByDesignationAndSubjectAsync(designationId, subjectId);

                // Check if the result is empty
                if (promotionList == null || !promotionList.Any())
                {
                    return NotFound("No employees found for the given designation and subject.");
                }


                return Ok(promotionList);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetSortedPromotionEligibleEmployees")]
        public async Task<IActionResult> GetSortedPromotionEligibleEmployeesAsync()
        {
            try
            {
                // Call the service method to get the sorted list of eligible employees
                var sortedEmployees = await _promotionService.GetSortedPromotionEligibleEmployeesAsync();

                // Return the sorted list as a response
                if (sortedEmployees == null || !sortedEmployees.Any())
                {
                    return NotFound("No promotion-eligible employees found.");
                }
                // Fetch all designations as DTOs
                var allDesignationsDTO = await _personalDetailsService.GetAllDesignationsAsync();

                // Map DTOs to entities
                var allDesignations = allDesignationsDTO.Select(dto => new Designation
                {
                    DesignationID = dto.DesignationID,
                    DesignationText = dto.DesignationName,
                    // Map other necessary fields as needed
                }).ToList();
                // Map the employees to DTOs
                var promotionRequestDTOs = sortedEmployees
                    .Select(tr => tr.GetEmployeePromotionListDTO(allDesignations))
                    .ToList();

                

                return Ok(promotionRequestDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 status code
                // Ideally, use a logger to log the exception (not shown here for brevity)
                return StatusCode(500, $"An error occurred while fetching promotion-eligible employees: {ex.Message}");
            }
        }
    

        [HttpPost("CreatePromotionRequest")]
        public async Task<IActionResult> CreatePromotionRequest([FromBody] CreatePromotionRequestDTO createPromotionRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Convert DTO to entity model
                var leaveRequest = createPromotionRequest.ToCreatePromotionRequest();

                // Inline check for an existing active Leave request for the same employee
                var existingRequest = await _context.Promotions
                    .AnyAsync(tr => tr.EmployeeID == leaveRequest.EmployeeID && tr.Status.StatusText == "Pending");

                if (existingRequest)
                {
                    // Return a conflict response if a Leave request already exists
                    return Conflict(new { message = "An active promotions request already exists for this employee.", status = 409 });
                }



                // Convert DTO to LeaveRequest entity using the mapper
                var convertedpromotion = createPromotionRequest.ToCreatePromotionRequest();

                // Create the Leave request via the service
                var newpromotionRequest = await _promotionService.CreatePromotionRequestAsync(convertedpromotion);

                if (newpromotionRequest == null)
                {
                    _logger.LogWarning("Failed to create promotion promotionrequest.");
                    return NotFound(new { message = "Failed to create promotion Request." });
                }

                // Return a success message with created Leave request details
                return Ok(new
                {
                    message = "Promotion request created successfully",
                    status = 200,
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while creating the Leave request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllTeacherPromotionRequests")]
        public async Task<IActionResult> GetAllTeacherPromotionRequests()
        {
            var promotionRequests = await _promotionService.GetAllTeacherPromotionRequestAsync();


            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var promotionsRequestDTOs = promotionRequests
                .Select(tr => tr.ToGetPromotionRequestDTO())
                .ToList();

            return Ok(promotionsRequestDTOs);
        }

        [HttpGet("GetAllNonTeacherPromotionRequests")]
        public async Task<IActionResult> GetAllNonTeacherPromotionsRequests()
        {
            var promotionRequests = await _promotionService.GetAllNonTeacherPromotionRequestAsync();

            if (promotionRequests == null || !promotionRequests.Any())
            {
                return NotFound("No Promotion requests found.");
            }

            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var promotionRequestDTOs = promotionRequests
                .Select(tr => tr.ToGetPromotionRequestDTO())
                .ToList();

            return Ok(promotionRequestDTOs);
        }


        [HttpGet("GetAllPromotedNonTeacher")]
        public async Task<IActionResult> GetAllPromotedNonTeacher()
        {
            var promotionRequests = await _promotionService.GetAllPromotedNonTeacherAsync();

            if (promotionRequests == null || !promotionRequests.Any())
            {
                return NotFound("No Promotion requests found.");
            }

            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var promotionRequestDTOs = promotionRequests
                .Select(tr => tr.ToGetPromotionRequestDTO())
                .ToList();

            return Ok(promotionRequestDTOs);
        }

        [HttpGet("GetAllPromotedTeacher")]
        public async Task<IActionResult> GetAllPromotedTeacher()
        {
            var promotionRequests = await _promotionService.GetAllPromotedTeacherAsync();

            if (promotionRequests == null || !promotionRequests.Any())
            {
                return NotFound("No Promotion requests found.");
            }

            // Map each LeaveRequest to GetLeaveRequestDTO using the mapper
            var promotionRequestDTOs = promotionRequests
                .Select(tr => tr.ToGetPromotionRequestDTO())
                .ToList();

            return Ok(promotionRequestDTOs);
        }


        [HttpPatch("ApprovePromotionRequest/{id}")]
        public async Task<IActionResult> ApproveLeaveRequest(int id, [FromBody] ApprovePromotionRequestDTO approvePromotionRequestDTO)
        {
            try
            {
                var approvedPromotion = await _promotionService.ApprovePromotionRequestAsync(id, approvePromotionRequestDTO);

                if (approvedPromotion == null)
                {
                    return NotFound(new { message = "Promotion request not found or already approved", status = 404 });
                }

                return Ok(new { message = "Promotion request approved successfully", status = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the Promotion request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("RejectPromotionRequest/{id}")]
        public async Task<IActionResult> RejectTransfer(int id, [FromBody] RejectPromotionRequestDTO rejectPromotionRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var rejectPromotionRequest = rejectPromotionRequestDTO.ToRejectPromotionRequest();

                var rejectPromotion = await _promotionService.RejectPromotionRequestAsync(id, rejectPromotionRequest);

                if (rejectPromotion == null)
                {
                    return NotFound();
                }

                return StatusCode(200, new { message = "Promotion Rejected successfully", status = 200 });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message, status = 400 });
            }
        }

        [HttpGet("TeacherPromotionCompletedfilter")]
        public async Task<IActionResult> TeacherPromotionCompletedfilter(
    [FromQuery] int? schoolID,
    [FromQuery] int? designationID,
    [FromQuery] string? uniqueID,
    [FromQuery] DateTime? fromPromotionDate,
    [FromQuery] DateTime? toPromotionDate)
        {
            try
            {
                // Call the service to filter promotions
                var promotions = await _promotionService.FilterTeacherPromotionCompletedListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromPromotionDate,
                    toPromotionDate);

                if (!promotions.Any())
                {
                    // Return an empty list if no promotions match the criteria
                    return Ok(new List<GetPromotionRequestDTO>());
                }

                // Convert entities to DTOs
                var promotionDTOs = promotions
                    .Select(p => p.ToGetPromotionRequestDTO())
                    .ToList();

                return Ok(promotionDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not included here)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpGet("NonTeacherPromotionCompletedfilter")]
        public async Task<IActionResult> NonTeacherPromotionCompletedfilter(
    [FromQuery] int? schoolID,
    [FromQuery] int? designationID,
    [FromQuery] string? uniqueID,
    [FromQuery] DateTime? fromPromotionDate,
    [FromQuery] DateTime? toPromotionDate)
        {
            try
            {
                // Call the service to filter promotions
                var promotions = await _promotionService.FilterNonTeacherPromotionCompletedListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromPromotionDate,
                    toPromotionDate);

                if (!promotions.Any())
                {
                    // Return an empty list if no promotions match the criteria
                    return Ok(new List<GetPromotionRequestDTO>());
                }

                // Convert entities to DTOs
                var promotionDTOs = promotions
                    .Select(p => p.ToGetPromotionRequestDTO())
                    .ToList();

                return Ok(promotionDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not included here)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpGet("TeacherPromotionRequestedfilter")]
        public async Task<IActionResult> TeacherPromotionRequestedfilter(
    [FromQuery] int? schoolID,
    [FromQuery] int? designationID,
    [FromQuery] string? uniqueID,
    [FromQuery] DateTime? fromPromotionDate,
    [FromQuery] DateTime? toPromotionDate)
        {
            try
            {
                // Call the service to filter promotions
                var promotions = await _promotionService.FilterTeacherPromotionRequestListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromPromotionDate,
                    toPromotionDate);

                if (!promotions.Any())
                {
                    // Return an empty list if no promotions match the criteria
                    return Ok(new List<GetPromotionRequestDTO>());
                }

                // Convert entities to DTOs
                var promotionDTOs = promotions
                    .Select(p => p.ToGetPromotionRequestDTO())
                    .ToList();

                return Ok(promotionDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not included here)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpGet("NonTeacherPromotionRequestedfilter")]
        public async Task<IActionResult> NonTeacherPromotionRequestedfilter(
    [FromQuery] int? schoolID,
    [FromQuery] int? designationID,
    [FromQuery] string? uniqueID,
    [FromQuery] DateTime? fromPromotionDate,
    [FromQuery] DateTime? toPromotionDate)
        {
            try
            {
                // Call the service to filter promotions
                var promotions = await _promotionService.FilterNonTeacherPromotionRequestListAsync(
                    schoolID,
                    designationID,
                    uniqueID,
                    fromPromotionDate,
                    toPromotionDate);

                if (!promotions.Any())
                {
                    // Return an empty list if no promotions match the criteria
                    return Ok(new List<GetPromotionRequestDTO>());
                }

                // Convert entities to DTOs
                var promotionDTOs = promotions
                    .Select(p => p.ToGetPromotionRequestDTO())
                    .ToList();

                return Ok(promotionDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not included here)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("CreatePromotionRelinquishment")]
        public async Task<IActionResult> CreatePromotionRelinquishment([FromBody] CreatePromotionRelinquishmentDTO createPromotionRelinquishment)
        {
            var promotionRelinquishment = createPromotionRelinquishment.ToCreatePromotionRelinquishment();
            var result = await _promotionService.CreatePromotionRelinquishment(promotionRelinquishment);

            if (result is null) return NotFound("Please check the fields are valid");

            return Ok(new
            {
                StatusCode = 200,
                Message = "PromotionRelinquishment created successfully", 
            });
        }



        [HttpGet("GetAllPromotionRelinquishments")]
        public async Task<ActionResult<List<GetPromotionRelinquishmentDTO>>> GetAllPromotionRelinquishmentsAsync()
        {
            try
            {
                // Fetch the promotion relinquishments with related employee and document data
                var relinquishments = await _promotionService.GetAllPromotionRelinquishmentsAsync();

                // Fetch all designations to calculate the next designation
                var allDesignations = await _context.Designations.ToListAsync();

                // Mapping the relinquishments to DTOs, passing the list of all designations
                var dtos = relinquishments.Select(r => r.ToGetPromotionRelinquishmentDTO(allDesignations)).ToList();

                return Ok(dtos);  // Return the mapped list of DTOs
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }


        [HttpPut("ApprovePromotionRelinquishment/{id}")]
        public async Task<IActionResult> ApprovePromotionRelinquishment(int id, [FromBody] ApprovePromotionRelinquishmentDTO updateDTO)
        {
            var relinquishmentUpdate = updateDTO.ToApprovePromotionReliquishment();

            // Call service method to update the school
            var updatedRelinquishment = await _promotionService.ApprovePromotionRelinquishmentAsync(id, updateDTO);

            if (updatedRelinquishment == null)
            {
                return NotFound(); 
            }

            return StatusCode(200, new { message = "Approved successfully", status = 200 });
        }

        [HttpGet("GetPromotionRequestByEmployeeID/{employeeID}")]
        public async Task<IActionResult> GetPromotionRequestsByEmployeeID(int employeeID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotionRequests = await _promotionService.GetPromotionRequestByEmployeeIdAsync(employeeID);

            if (promotionRequests == null || !promotionRequests.Any())
            {
                return Ok(new { StatusCode = 204, Message = "No employee found" });
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var PromotionRequestDTOs = promotionRequests
                .Select(p => p.ToGetPromotionRequestDTO())
                .ToList();

            return Ok(PromotionRequestDTOs);
        }
        [HttpGet("GetTeachersPromotionRequestsBySchoolID/{schoolID}")]
        public async Task<IActionResult> GetTeachersPromotionRequestsBySchoolID(int schoolID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotionRequests = await _promotionService.GetTeachersPromotionRequestsBySchoolIDAsync(schoolID);

            if (promotionRequests == null)
            {
                return Ok(new { StatusCode = 204, Message = "No School found" });
            }

            if (!promotionRequests.Any())
            {
                return Ok(new List<GetPromotionRequestDTO>());
            }
            var PromotionRequestDTOs = promotionRequests
                .Select(tr => tr.ToGetPromotionRequestDTO())
                .ToList();

            return Ok(PromotionRequestDTOs);
        }
    }
}
