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
            if (pageIndex <= 0 || pageSize <= 0)
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

            return new PagedProjectsResponseDto
            {
                Projects = projects,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<int> GetTotalProjectCountAsync()
        {
            return await _context.Projects.CountAsync();
        }

        public Project GetProject(int projectId)
        {
            return _context.Projects
                .Include(project => project.ProjectManager)
                .Include(project => project.ProjectTasks)
                .FirstOrDefault(project => project.Id == projectId);
        }

        public Project CreateProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public void DeleteProject(int projectId)
        {

            var project = _context.Projects.FirstOrDefault(project => project.Id == projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
        }


        public Project UpdateProject(int? id, Project project)
        {
            var existingProject = _context.Projects.FirstOrDefault(project => project.Id == id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.StartDate = project.StartDate;
                existingProject.EndDate = project.EndDate;
                _context.SaveChanges();
            }
            return existingProject;
        }

    }
}
