using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Status Status { get; set; }

        //navigation properties
        // Foreign key for the Project Manager
        [ForeignKey("ProjectManager")]
        public int? ProjectManagerID { get; set; }
        public virtual ProjectManager? ProjectManager { get; set; }

        // Relationship: One Project can have multiple tasks
        public virtual ICollection<ProjectTask>? ProjectTasks { get; set; }

    }
}
