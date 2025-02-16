using WorkSphere.Server.Model;

namespace WorkSphere.Server.Dtos
{
    public class PagedSalaryResponseDto
    {
        public List<Salary> Salaries { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
