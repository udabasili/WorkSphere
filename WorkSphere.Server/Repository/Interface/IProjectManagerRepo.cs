using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository.Interface
{
    public interface IProjectManagerRepo
    {
        Task<ProjectManager> GetProjectManagerAsync(int id);

        Task<ProjectManager> AddProjectManagerAsync(ProjectManager projectManager);

        Task<ProjectManager> UpdateProjectManagerAsync(int id, ProjectManager projectManager);

        Task<ProjectManager> DeleteProjectManagerAsync(int id);

        Task<PagedProjectManagerResponseDto> GetPagedProjectManagersAsync(int pageIndex, int pageSize);

        Task<bool> ProjectManagerEmailExists(ProjectManager projectManager);
    }
}
