using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Services
{
    public interface IProjectTaskService
    {
        public Task<ProjectTasksResponseDto> GetProjectTasksAsync(int projectId);

        public Task<List<ProjectTaskDto>> BulkUpdateProjectTasks(ProjectTaskBulkInsertDto projectTaskBulkInsertDto);


    }
}
