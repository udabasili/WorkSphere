using System.ComponentModel.DataAnnotations;
using TastyTreats.Types;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    public class ProjectManagerService : IProjectManagerService
    {

        public readonly IProjectManagerRepo _projectManagerRepo;

        public ProjectManagerService(IProjectManagerRepo projectManagerRepo)
        {
            _projectManagerRepo = projectManagerRepo;
        }

        public async Task<PagedProjectManagerResponseDto> PagedProjectManagerResponseDto(int pageIndex, int pageSize)
        {
            return await _projectManagerRepo.GetPagedProjectManagersAsync(pageIndex, pageSize);
        }

        public async Task<ProjectManager> AddProjectManager(ProjectManager projectManager)
        {
            if (await Validate(projectManager))
            {
                return await _projectManagerRepo.AddProjectManagerAsync(projectManager);
            }
            return projectManager;
        }

        public async Task<ProjectManager> DeleteProjectManager(int id)
        {
            return await _projectManagerRepo.DeleteProjectManagerAsync(id);
        }

        public async Task<ProjectManager> GetProjectManager(int id)
        {
            return await _projectManagerRepo.GetProjectManagerAsync(id);
        }

        public async Task<ProjectManager> UpdateProjectManager(int id, ProjectManager projectManager)
        {
            if (await Validate(projectManager))
            {
                return await _projectManagerRepo.UpdateProjectManagerAsync(id, projectManager);
            }
            return projectManager;
        }

        //validation

        private async Task<bool> Validate(ProjectManager projectManager)
        {
            await ValidateEmail(projectManager);
            ValidateModel(projectManager);

            return projectManager.Errors.Count == 0;
        }

        private async Task<bool> ValidateEmail(ProjectManager projectManager)
        {
            if (await _projectManagerRepo.ProjectManagerEmailExists(projectManager))
            {

                projectManager.AddError("Email already exists", ErrorType.Business);
                return false;
            }
            return true;

        }

        private void ValidateModel(ProjectManager projectManager)
        {
            List<ValidationResult> results = new();
            Validator.TryValidateObject(projectManager, new ValidationContext(projectManager), results, true);

            foreach (ValidationResult e in results)
                projectManager.AddError(e.ErrorMessage, ErrorType.Model);
        }

    }
}


