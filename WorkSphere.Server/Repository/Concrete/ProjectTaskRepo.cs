using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository
{
    public class ProjectTaskRepo : IProjectTaskRepo
    {
        private readonly WorkSphereDbContext _context;
        private readonly IMapper _mapper;

        public ProjectTaskRepo(WorkSphereDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProjectTask>> GetProjectTasksByProjectIdAsync(int projectId)
        {
            var projectTasks = await _context.ProjectTasks
                .Where(projectTask => projectTask.ProjectID == projectId)
                .OrderBy(projectTask => projectTask.Order)
                .ToListAsync();

            //if no tasks return an empty array
            if (!projectTasks.Any())
            {
                projectTasks = new List<ProjectTask>();
            }
            return projectTasks;
        }

        public async Task<List<TeamMemberDto>> GetTeamMembersByProjectTasksAsync(List<int?>? employeeIds)
        {
            return await _context.Employees
                .Where(employee => employeeIds.Contains(employee.Id))
                .Select(employee => new TeamMemberDto
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                })
                .ToListAsync();
        }


        public async Task<ProjectTasksResponseDto> GetProjectTasksAsync(int projectId)
        {
            var projectTasks = await _context.ProjectTasks
                .Where(projectTask => projectTask.ProjectID == projectId)
                .OrderBy(projectTask => projectTask.Order)
                .Select(projectTask => new
                {
                    ProjectTask = projectTask,
                    Employee = projectTask.Employee
                })
                .ToListAsync();

            if (!projectTasks.Any())
            {
                return new ProjectTasksResponseDto
                {
                    ProjectTasks = new List<ProjectTaskDto>(),
                    ProjectTeamMembers = new List<TeamMemberDto>()
                };
            }

            var projectTasksDto = projectTasks.Select(pt => new ProjectTaskDto
            {
                Id = pt.ProjectTask.Id,
                Name = pt.ProjectTask.Name,
                Description = pt.ProjectTask.Description,
                ProjectID = pt.ProjectTask.ProjectID,
                EmployeeIDs = projectTasks
                    .Where(task => task.ProjectTask.Name == pt.ProjectTask.Name && task.ProjectTask.EmployeeID.HasValue)
                    .Select(task => task.ProjectTask.EmployeeID.Value)
                    .Distinct()
                    .ToList(),
                Order = pt.ProjectTask.Order,
                Status = pt.ProjectTask.Status.ToString(),
                Duration = pt.ProjectTask.Duration,
                NumOfTeamMembers = projectTasks
                    .Count(task => task.ProjectTask.Name == pt.ProjectTask.Name && task.ProjectTask.EmployeeID.HasValue)
            }).ToList();

            var projectTeamMembers = projectTasks
                .Where(pt => pt.Employee != null)
                .Select(pt => new TeamMemberDto
                {
                    Id = pt.Employee.Id,
                    FirstName = pt.Employee.FirstName,
                    LastName = pt.Employee.LastName
                })
                .Distinct()
                .ToList();

            return new ProjectTasksResponseDto
            {
                ProjectTasks = projectTasksDto,
                ProjectTeamMembers = projectTeamMembers
            };

        }

        public async Task<List<ProjectTaskDto>> BulkUpdateProjectTasks(ProjectTaskBulkInsertDto projectTaskBulkInsertDto)
        {

            var projectTasks = _mapper.Map<List<ProjectTask>>(projectTaskBulkInsertDto.Tasks);
            //remove id because it is auto generated
            projectTasks.ForEach(task => task.Id = 0);
            _context.ProjectTasks.AddRange(projectTasks);
            await _context.SaveChangesAsync();

            return _mapper.Map<List<ProjectTaskDto>>(projectTasks);
        }

        // set tasks for employees
        public async Task<List<ProjectTaskDto>> SetTasksForEmployees(ProjectTaskBulkInsertDto projectTaskBulkInsertDto)
        {
            var projectTasks = _mapper.Map<List<ProjectTask>>(projectTaskBulkInsertDto.Tasks);
            //remove id because it is auto generated
            _context.ProjectTasks.AddRange(projectTasks);
            await _context.SaveChangesAsync();
            return _mapper.Map<List<ProjectTaskDto>>(projectTasks);
        }


    }
}
