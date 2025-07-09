using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TaskManagerApp.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // plus d'attribut Required → pas d'erreur de validation côté ModelBinder
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }
    }
}
