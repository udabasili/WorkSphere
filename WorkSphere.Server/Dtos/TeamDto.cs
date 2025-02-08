using WorkSphere.Server.Model;

namespace WorkSphere.Server.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }


        public List<TeamMemberDto> TeamMembers { get; set; }

        public ProjectManager ProjectManager { get; set; }

        public int NumOfCompletedTasks { get; set; }

        public int NumOfPendingTasks { get; set; }
    }
}
