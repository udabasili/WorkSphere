namespace WorkSphere.Server.Model
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public enum Role
    {
        Admin,
        Agent,
        Customer
    }
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public Role Role { get; set; } // Employee, Manager, Admin
    }

    public class EmployeeUser : ApplicationUser
    {
        [ForeignKey("EmployeeID")]
        public int? EmployeeID { get; set; }

        public virtual Employee? Employee { get; set; }

    }

    public class ProjectManagerUser : ApplicationUser
    {
        [ForeignKey("ManagerID")]
        public int? ManagerID { get; set; }

        // One ManagerUser can be associated with one Manager
        public virtual ProjectManager? ProjectManager { get; set; }


    }

}
