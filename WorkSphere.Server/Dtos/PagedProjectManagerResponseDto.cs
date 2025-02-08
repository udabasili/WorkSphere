namespace WorkSphere.Server.Dtos
{
    public class PagedProjectManagerResponseDto
    {
        public List<ProjectManagerDto> ProjectManagers { get; set; }

        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
