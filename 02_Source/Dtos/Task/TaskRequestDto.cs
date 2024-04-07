using Common.Attributes;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Task
{
    public class TaskRequestDto
    {
        [Required(ErrorMessage = "The title is required!")]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The priority is required!")]
        [EnumDataType(typeof(PriorityEnum), ErrorMessage = "Invalid priority value!")]
        public string Priority { get; set; }

        [FutureDate]
        public DateTime DueDate { get; set; }
    }
}
