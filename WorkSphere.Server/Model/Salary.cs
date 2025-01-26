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

        //Navigation Properties
        // Relationship: One Salary is tied to one Employee
        public virtual Employee? Employee { get; set; }
        public int? EmployeeID { get; set; }
    }
}
