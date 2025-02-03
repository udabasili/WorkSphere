namespace WorkSphere.Server.Dtos
{
    public class ProjectTasksResponseDto
    {
        public List<ProjectTaskDto> ProjectTasks { get; set; }

        public List<TeamMemberDto> ProjectTeamMembers { get; set; }
    }
}
