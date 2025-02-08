using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;
using WorkSphere.Server.Repository.Interface;

namespace WorkSphere.Server.Repository
{
    public class ProjectManagerRepo : IProjectManagerRepo
    {
        public readonly WorkSphereDbContext _context;

        public ProjectManagerRepo(WorkSphereDbContext context)
        {
            _context = context;
        }

        private async Task<List<ProjectManagerDto>> GetProjectManagers(int pageIndex = 0, int pageSize = 0)
        {
            if (pageSize == 0)
            {
                return await _context.ProjectManagers.Select(projectManager => new ProjectManagerDto
                {
                    Id = projectManager.Id,
                    FirstName = projectManager.FirstName,
                    LastName = projectManager.LastName,
                    Email = projectManager.Email,
                    Salary = projectManager.Salary,
                }).ToListAsync();
            }

            var projectManagers = await _context.ProjectManagers.Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return projectManagers.Select(projectManager => new ProjectManagerDto
            {
                Id = projectManager.Id,
                FirstName = projectManager.FirstName,
                LastName = projectManager.LastName,
                Email = projectManager.Email,
                Salary = projectManager.Salary,
            }).ToList();
        }

        private async Task<int> GetProjectManagersCount()
        {
            return await _context.ProjectManagers.CountAsync();
        }

        public async Task<PagedProjectManagerResponseDto> GetPagedProjectManagersAsync(int pageIndex, int pageSize)
        {
            var projectManagers = await GetProjectManagers(pageIndex, pageSize);
            var totalCount = await GetProjectManagersCount();
            return new PagedProjectManagerResponseDto
            {
                ProjectManagers = projectManagers,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<ProjectManager> GetProjectManagerAsync(int id)
        {
            var projectManager = await _context.ProjectManagers
                .Include(projectManager => projectManager.Salary)
                .FirstOrDefaultAsync(projectManager => projectManager.Id == id);
            return projectManager;
        }

        public async Task<ProjectManager> AddProjectManagerAsync(ProjectManager projectManager)
        {
            _context.ProjectManagers.Add(projectManager);
            await _context.SaveChangesAsync();
            return projectManager;
        }


        public async Task<ProjectManager> UpdateProjectManagerAsync(int id, ProjectManager projectManager)
        {

            _context.Entry(projectManager).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return projectManager;

        }


        public async Task<ProjectManager> DeleteProjectManagerAsync(int id)
        {
            var projectManager = await _context.ProjectManagers.FindAsync(id);
            _context.ProjectManagers.Remove(projectManager);
            await _context.SaveChangesAsync();
            return projectManager;
        }

        public async Task<bool> ProjectManagerEmailExists(ProjectManager projectManager)
        {

            return await _context.ProjectManagers.AnyAsync(e => e.Email == projectManager.Email
            && e.Id != projectManager.Id);
        }

    }

}
