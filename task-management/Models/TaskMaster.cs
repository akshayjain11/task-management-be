using System.ComponentModel.DataAnnotations;

namespace task_management.Models
{
    public class TaskMaster
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public DateTime CreatedDate { get; set;}
        public DateTime UpdatedDate { get; set;}
        [Required]
        public int CreatedBy { get; set; }
        public int AssignTo { get; set; }

        public string? Attachment { get; set; }

    }
}
