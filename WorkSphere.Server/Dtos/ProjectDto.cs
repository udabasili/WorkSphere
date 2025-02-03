namespace WorkSphere.Server.Dtos
{

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int ProjectManagerID { get; set; }
        public List<ProjectTaskDto> ProjectTasks
        {
            get; set;
        }
    }
}
