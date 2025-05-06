using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<TaskItem>> PostTask(TaskItem taskItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Set creation timestamp and creator
                taskItem.CreatedAt = DateTime.UtcNow;
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                _context.Tasks.Add(taskItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New task created with ID: {TaskId} by user: {UserId}", taskItem.Id, userId);

                return CreatedAtAction(nameof(PostTask), new { id = taskItem.Id }, taskItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new task");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
} 