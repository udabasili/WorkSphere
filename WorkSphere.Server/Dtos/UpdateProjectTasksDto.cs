using System.ComponentModel.DataAnnotations;

namespace WorkSphere.Server.Dtos
{
    public class UpdateProjectTasksDto
    {
        [Required]
        public List<ProjectTaskDto> Tasks { get; set; }
    }
}
