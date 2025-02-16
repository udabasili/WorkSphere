using WorkSphere.Model;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _projectRepo;

        public ProjectService(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public async Task<PagedProjectsResponseDto> GetPagedProjectsAsync(int pageIndex, int pageSize)
        {
            return await _projectRepo.GetPagedProjectsAsync(pageIndex, pageSize);
        }

        public async Task<int> GetTotalProjectCountAsync()
        {
            return await _projectRepo.GetTotalProjectCountAsync();
        }


        public async Task<Project> GetProjectAsync(int projectId)
        {
            return await _projectRepo.GetProjectAsync(projectId);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            return await _projectRepo.CreateProjectAsync(project);
        }

        public async Task<Project> UpdateProjectAsync(int id, Project project)
        {
            return await _projectRepo.UpdateProjectAsync(id, project);
        }

        public async Task<Project> DeleteProjectAsync(int projectId)
        {

            return await _projectRepo.DeleteProjectAsync(projectId);
        }


    }
}
