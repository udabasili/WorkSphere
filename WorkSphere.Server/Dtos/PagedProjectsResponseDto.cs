using WorkSphere.Model;

namespace WorkSphere.Server.Dtos
{
    public class PagedProjectsResponseDto
    {
        public List<Project> Projects { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
