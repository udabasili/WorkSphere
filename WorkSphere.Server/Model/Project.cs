using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WorkSphere.Server.Model;

namespace WorkSphere.Model
{
    public enum Status
    {
        Active,
        Inactive,
        Completed
    }

    public class Project : BaseEntity
    {
        [Required(ErrorMessage = "Project name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [NotMapped]
        public DateTime EndDate => StartDate.AddDays(ProjectTasks?.Sum(task => task.Duration) ?? 0);

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        // One-to-Many: A ProjectManager manages multiple projects
        public int? ProjectManagerID { get; set; }
        public virtual ProjectManager? ProjectManager { get; set; }

        // One-to-Many: A project can have multiple tasks
        public virtual ICollection<ProjectTask>? ProjectTasks { get; set; }

        // Many-to-Many: A project can have multiple employees
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
