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
        // Relationship: A ProjectManager manages multiple projects
        [JsonIgnore]
        public virtual ICollection<Project>? ManagedProjects { get; set; }

        //salary
        [ForeignKey("Salary")]
        public int? SalaryID { get; set; }
        public virtual Salary? Salary { get; set; }


    }

}
