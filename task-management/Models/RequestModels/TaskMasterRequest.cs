using System.ComponentModel.DataAnnotations;
using task_management.Utils.Attributes;

namespace task_management.Models.RequestModels
{
    public class TaskMasterRequest
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public int AssignTo { get; set; }
        [Base64Image]
        public string? Attachment { get; set; }
    }
}
