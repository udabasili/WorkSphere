using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkSphere.Model;

namespace WorkSphere.Server.Model
{
    public class Employee : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required]
        public DateTime EmploymentDate { get; set; }
        [DataType(DataType.Date)]

        [NotMapped]
        public string? FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        //navigation properties
        // one to one relationship between Employee and Salary
        [ForeignKey("Salary")]
        public int? SalaryID { get; set; }
        public virtual Salary? Salary { get; set; }

        // Relationship: One Employee can have multiple tasks assigned
        public virtual ICollection<ProjectTask>? ProjectTasks { get; set; }



    }
}
