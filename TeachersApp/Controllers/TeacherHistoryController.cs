using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherHistoryDTO;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Mapper;
using TeachersApp.Services.Repositories;

namespace TeachersApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherHistoryController : ControllerBase
    {
        private readonly ITeacherHistoryService _teacherHistoryService;

        public TeacherHistoryController (ITeacherHistoryService teacherHistoryService)
        {
            _teacherHistoryService = teacherHistoryService;
        }
        [Authorize]
        [HttpGet("GetHistoryListById/{employeeId}")]
        public async Task<ActionResult<List<GetTeacherHistoryDTO>>> GetHistoryListById(int employeeId)
        {
            try
            {
                // Call the service method to get the school details
                var historyList = await _teacherHistoryService.GetHistoriesByIdAsync(employeeId);

                if (!historyList.Any())
                {
                     return Ok(new List<GetTeacherHistoryDTO>());
                }

                // Return 200 OK with the school details in the response
                var historyDtoList = historyList.Select(s => s.ToGetTeacherHistoryDTO()).ToList();
                return Ok(historyDtoList);
            }

            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An error occurred while retrieving school details.", error = ex.Message });
            }
        }

    }
}
