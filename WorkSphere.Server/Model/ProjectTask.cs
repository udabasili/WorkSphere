using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkSphere.Model;

namespace WorkSphere.Server.Model
{
    public class ProjectTask : BaseEntity
    {
        [Required(ErrorMessage = "Task name is required")]
        public string? Name { get; set; }
        public string? Description { get; set; }

        //navigation properties
        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }
        public virtual Employee? Employee { get; set; }

        // Foreign key for the Project the task belongs to
        [ForeignKey("Project")]
        public int? ProjectID { get; set; }
        public virtual Project? Project { get; set; }
    }
}
