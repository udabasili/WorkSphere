using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository
{
    public interface IProjectTaskRepo
    {
        public Task<List<ProjectTask>> GetProjectTasksByProjectIdAsync(int projectId);
        public Task<List<TeamMemberDto>> GetTeamMembersByProjectTasksAsync(List<int?>? employeeIds);

        public Task<List<ProjectTaskDto>> BulkUpdateProjectTasks(ProjectTaskBulkInsertDto projectTaskBulkInsertDto);

        public Task<ProjectTasksResponseDto> GetProjectTasksAsync(int projectId);

    }
}
