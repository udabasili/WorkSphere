namespace WorkSphere.Server.Dtos
{
    public class PagedEmployeeResponseDto
    {
        public List<EmployeeDto> Employees { get; set; }

        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
