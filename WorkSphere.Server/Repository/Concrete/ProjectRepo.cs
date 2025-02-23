using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Model;
using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Repository.Concrete
{
    public class ProjectRepo : IProjectRepo
    {

        private readonly WorkSphereDbContext _context;

        public ProjectRepo(WorkSphereDbContext context)
        {
            _context = context;
        }

        public async Task<PagedProjectsResponseDto> GetPagedProjectsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var projects = new List<Project>();
            if (pageSize == 0)
            {
                projects = await _context.Projects
               .Include(project => project.ProjectManager)
               .Include(project => project.ProjectTasks)
               .ToListAsync();

            }
            projects = await _context.Projects
                .Include(project => project.ProjectManager)
                .Include(project => project.ProjectTasks)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalProjects = await GetTotalProjectCountAsync();

            return new PagedProjectsResponseDto
            {
                Projects = projects,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalProjects
            };
        }

        public async Task<int> GetTotalProjectCountAsync()
        {
            return await _context.Projects.CountAsync();
        }

        public async Task<Project> GetProjectAsync(int projectId)
        {
            if (projectId == 0)
            {
                return null;
            }
            return await _context.Projects
                .Include(project => project.ProjectManager)
                .Include(project => project.ProjectTasks)
                .FirstOrDefaultAsync(project => project.Id == projectId);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> DeleteProjectAsync(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return project;
        }


        public async Task<Project> UpdateProjectAsync(int? id, Project project)
        {
            var existingProject = await _context.Projects.FirstOrDefaultAsync(project => project.Id == id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.StartDate = project.StartDate;
                await _context.SaveChangesAsync();
            }
            return existingProject;
        }


    }
}
