using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkSphere.Model;

namespace WorkSphere.Server.Model
{
    public class Salary : BaseEntity
    {
        [Required]
        [Precision(18, 2)]
        public decimal BasicSalary { get; set; }

        [Precision(18, 2)]
        public decimal? Bonus { get; set; }

        [Precision(18, 2)]
        public decimal? Deductions { get; set; }

        // Calculated Total Salary
        [NotMapped]
        public decimal TotalSalary => (BasicSalary + (Bonus ?? 0)) - (Deductions ?? 0);

        // One-to-One: Salary is tied to an Employee
        public int? EmployeeID { get; set; }

        // [JsonIgnore]
        public virtual Employee? Employee { get; set; }

        // One-to-One: Salary is tied to a Project Manager
        public int? ProjectManagerID { get; set; }

        // [JsonIgnore]
        public virtual ProjectManager? ProjectManager { get; set; }
    }
}
