using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskItemDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_]+$", ErrorMessage = "Title can only contain letters, numbers, spaces, hyphens, and underscores")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,!?]+$", ErrorMessage = "Description contains invalid characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(TaskStatus), ErrorMessage = "Invalid status value")]
        public TaskStatus Status { get; set; }
    }
} 