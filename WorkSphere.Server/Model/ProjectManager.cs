using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WorkSphere.Model;

namespace WorkSphere.Server.Model
{
    public class ProjectManager : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime EmploymentDate { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // One-to-Many: A ProjectManager manages multiple projects
        [JsonIgnore]
        public virtual ICollection<Project>? ManagedProjects { get; set; }

        // One-to-One: ProjectManager has a salary
        public int? SalaryID { get; set; }
        public virtual Salary? Salary { get; set; }
    }

}
