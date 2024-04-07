using Common.Attributes;
using Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dtos.Task
{
    public class TaskDto
    {
        [Required(ErrorMessage = "The title is required!")]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string? Description { get; set; }

        
        [Required(ErrorMessage = "The priority is required!")]
        [EnumDataType(typeof(PriorityEnum), ErrorMessage = "Invalid priority value!")]
        public PriorityEnum Priority { get; set; }

        [FutureDate]
        public DateTime DueDate { get; set; }
    }
}
