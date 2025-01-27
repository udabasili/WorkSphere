namespace WorkSphere.Server.Model
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public enum Role
    {
        Admin,
        ProjectManager,
        Employee
    }
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string Role { get; set; }
    }

    public class EmployeeUser : ApplicationUser
    {
        [ForeignKey("EmployeeID")]
        public int? EmployeeID { get; set; }

        public virtual Employee? Employee { get; set; }

    }

    public class ProjectManagerUser : ApplicationUser
    {
        [ForeignKey("ProjectManagerID")]
        public int? ProjectManagerId { get; set; }

        // One ManagerUser can be associated with one Manager
        public virtual ProjectManager? ProjectManager { get; set; }


    }

}
