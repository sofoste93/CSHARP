using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TaskManagerApp.Models;

public enum TaskPriority { Low = 0, Normal = 1, High = 2, Critical = 3 }

public class TaskItem
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = "";

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Normal;

    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = "";

    [ForeignKey(nameof(UserId))]
    public virtual IdentityUser? User { get; set; }
}
