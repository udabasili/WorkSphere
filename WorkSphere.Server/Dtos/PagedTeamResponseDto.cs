namespace WorkSphere.Server.Dtos
{
    public class PagedTeamResponseDto
    {
        public List<TeamDto> Teams { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

}
