using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WorkSphere.Model;

namespace WorkSphere.Server.Model
{
    public class ProjectTask : BaseEntity
    {
        [Required(ErrorMessage = "Task name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public int? Order { get; set; }

        [Required]
        public int Duration { get; set; } // Duration in days

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        // One-to-Many: A task is assigned to an employee
        public int? EmployeeID { get; set; }

        [JsonIgnore]
        public virtual Employee? Employee { get; set; }

        // One-to-Many: A task belongs to a project
        public int? ProjectID { get; set; }

        [JsonIgnore]
        public virtual Project? Project { get; set; }
    }
}
