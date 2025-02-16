using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Services
{
    public interface IProjectManagerService
    {
        Task<ProjectManager> GetProjectManager(int id);

        Task<ProjectManager> AddProjectManager(ProjectManager projectManager);

        Task<ProjectManager> UpdateProjectManager(int id, ProjectManager projectManager);

        Task<ProjectManager> DeleteProjectManager(int id);

        Task<PagedProjectManagerResponseDto> PagedProjectManagerResponseDto(int pageIndex, int pageSize);
    }
}
