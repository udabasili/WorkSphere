using WorkSphere.Model;
using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Repository
{
    public interface IProjectRepo
    {
        public Task<PagedProjectsResponseDto> GetPagedProjectsAsync(int pageIndex, int pageSize);
        public Task<int> GetTotalProjectCountAsync();

        public Project GetProject(int projectId);

        public Project CreateProject(Project project);

        public Project UpdateProject(int? id, Project project);

        public void DeleteProject(int projectId);
    }
}
