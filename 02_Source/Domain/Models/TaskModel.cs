using System.ComponentModel.DataAnnotations;
using Common.Constants;

namespace Domain.Models
{
    public class TaskModel
    {
        [Key]
        [Required]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string? Description { get; set; }

        [Required]
        public PriorityEnum Priority { get; set; }

        public DateTime DueDate { get; set; }
    }
}
