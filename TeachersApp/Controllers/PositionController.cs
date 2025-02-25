using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.PositionDTO;
using TeachersApp.Entity.ModelDTO.SchoolTypeDTO;
using TeachersApp.Entity.Models;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }


        #region GetAllPositionCount


        [HttpGet("GetAllPositionCount")]
        public async Task<IActionResult> GetAllPositionCount()
        {
            try
            {
                var positionCount = await _positionService.GetTotalVacantPositionCountAsync();
                return Ok(positionCount);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving the position count.");
            }
        }

        #endregion

        [HttpPost("AddNewSchoolPosition")]
        public async Task<IActionResult> CreateNewSchoolPosition([FromBody] CreateNewPositionDTO addPositionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var schoolPosition = addPositionDTO.AddSchoolPositionDTO();
            var result = await _positionService.CreateNewPositionAsync(schoolPosition);

            if (result is null) return NotFound("Please check the fields are valid");

            return Ok(new { StatusCode = 200, Message = "New Position created" });
        }

        [HttpGet("GetAllNewAndVacantSchoolPositions")]
        public async Task<IActionResult> GetAllNewAndVacantSchoolPositions()
        {
            var positions = await _positionService.GetAllVacantAndNewSchoolPositionsAsync();

            if (positions == null || !positions.Any())
            {
                return NotFound("No positions found.");
            }

            // Map each TransferRequest to GetTransferRequestDTO using the mapper
            var transferRequestDTOs = positions
                .Select(tr => tr.ToGetVacantAndNewSchoolPositionsDTO())
                .ToList();

            return Ok(transferRequestDTOs);
        }
        [HttpPost("HandleRetiredVacancies")]
        public async Task<IActionResult> HandleRetiredVacancies()
        {
            try
            {
                // Call the service to handle retired vacancies
                var count = await _positionService.HandleVacanciesForEmployeesAsync();

                // Return appropriate responses based on the result
                if (count > 0)
                {
                    return Ok(new { Message = $"{count} vacancies created successfully." });
                }

                return Ok(new { Message = "No vacancies created" });
            }
            catch (InvalidOperationException ex)
            {
                // Handle known errors like missing status IDs
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("DeleteSchoolPosition/{id}")]
        public async Task<IActionResult> DeleteSchoolPosition(int id)
        {
            var result = await _positionService.DeleteSchoolPositionAsync(id);
            if (!result)
            {
                return NotFound("No positions found.");
            }
            return Ok(new { StatusCode = 200, Message = " Position Deleted" });
        }

    }

}

