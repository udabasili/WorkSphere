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


        public async Task<Project> GetProject(int projectId)
        {
            return _projectRepo.GetProject(projectId);
        }

        public Project CreateProject(Project project)
        {
            return _projectRepo.CreateProject(project);
        }

        public void DeleteProject(int projectId)
        {

            _projectRepo.DeleteProject(projectId);
        }

        public Project UpdateProject(int id, Project project)
        {
            return _projectRepo.UpdateProject(id, project);
        }


    }
}
