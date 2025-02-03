using WorkSphere.Server.Model;

namespace WorkSphere.Server.Dtos
{
    public class ProjectTaskBulkInsertDto
    {
        public int ProjectId { get; set; }
        public List<ProjectTask> Tasks { get; set; }
    }
}
