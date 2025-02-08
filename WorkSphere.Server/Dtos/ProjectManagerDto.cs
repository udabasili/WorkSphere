using WorkSphere.Server.Model;

namespace WorkSphere.Server.Dtos
{
    public class ProjectManagerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string Email { get; set; }

        public DateTime EmploymentDate { get; set; }

        public virtual Salary Salary { get; set; }

    }
}
