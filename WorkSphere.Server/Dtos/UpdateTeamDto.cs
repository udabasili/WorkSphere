namespace WorkSphere.Server.Dtos
{
    public class UpdateTeamDto
    {
        public int projectId { get; set; }
        public int projectManagerId { get; set; }
        public List<int> teamMembers { get; set; } = new List<int>();

    }
}
