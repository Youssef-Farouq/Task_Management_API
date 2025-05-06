using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,TaskCreator")]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ApplicationDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] TaskItemDto taskDto)
        {
            try
            {
                if (taskDto == null)
                {
                    _logger.LogWarning("Received null task item in POST request");
                    return BadRequest("Task item cannot be null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state in POST request");
                    return BadRequest(ModelState);
                }

                // Validate title for potential XSS
                if (ContainsHtmlTags(taskDto.Title) || ContainsHtmlTags(taskDto.Description))
                {
                    _logger.LogWarning("Potential XSS attack detected in task data");
                    return BadRequest("Invalid input: HTML tags are not allowed");
                }

                var taskItem = new TaskItem
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    Status = taskDto.Status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                _context.Tasks.Add(taskItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Task created successfully with ID: {taskItem.Id} by user {User.Identity?.Name}");

                // Return 201 Created with the created resource
                return StatusCode(201, new
                {
                    Id = taskItem.Id,
                    Title = taskItem.Title,
                    Status = taskItem.Status,
                    CreatedAt = taskItem.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private bool ContainsHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            
            // Simple HTML tag detection
            return input.Contains("<") && input.Contains(">");
        }
    }
} 