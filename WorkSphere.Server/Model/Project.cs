using System.ComponentModel.DataAnnotations;

namespace WorkSphere.Model
{
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
    }
}
