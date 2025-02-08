namespace WorkSphere.Server.Dtos
{
    public class ProjectTaskDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeID { get; set; }
        public int? Order { get; set; }
        public string Status { get; set; }
        public int Duration { get; set; }

        public int NumOfTeamMembers { get; set; }

    }
}
