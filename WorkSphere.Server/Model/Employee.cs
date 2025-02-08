using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkSphere.Model;

namespace WorkSphere.Server.Model
{
    public class Employee : BaseEntity
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.Date)]
        public DateTime EmploymentDate { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // One-to-One Relationship with Salary
        public int? SalaryID { get; set; }
        public virtual Salary? Salary { get; set; }

        // One-to-Many: Employee can be assigned multiple tasks
        public virtual ICollection<ProjectTask>? ProjectTasks { get; set; }

        // Many-to-Many: Employee can be assigned to multiple projects
        public virtual ICollection<Project>? Projects { get; set; }
    }
}
