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
            return await _context.ProjectTasks
                .Where(projectTask => projectTask.ProjectID == projectId)
                .OrderBy(projectTask => projectTask.Order)
                .ToListAsync();
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
            var projectTasks = await this.GetProjectTasksByProjectIdAsync(projectId);
            var employeeIds = projectTasks.Select(task => task.EmployeeID).Distinct().ToList();
            var projectTeamMembers = await this.GetTeamMembersByProjectTasksAsync(employeeIds);

            var projectTasksDto = projectTasks.Select(projectTask => new ProjectTaskDto
            {
                Id = projectTask.Id,
                Name = projectTask.Name,
                Description = projectTask.Description,
                ProjectID = projectTask.ProjectID,
                EmployeeID = projectTask.EmployeeID,
                Order = projectTask.Order,
                Status = projectTask.Status.ToString(),
                NumOfTeamMembers = projectTeamMembers.Count(employee => employee.Id == projectTask.EmployeeID)
            }).ToList();

            return new ProjectTasksResponseDto
            {
                ProjectTasks = projectTasksDto,
                ProjectTeamMembers = _mapper.Map<List<TeamMemberDto>>(projectTeamMembers)
            };
        }

        public Task<List<ProjectTaskDto>> BulkUpdateProjectTasks(ProjectTaskBulkInsertDto projectTaskBulkInsertDto)
        {

            var projectTasks = _mapper.Map<List<ProjectTask>>(projectTaskBulkInsertDto.Tasks);
            //remove id because it is auto generated
            projectTasks.ForEach(task => task.Id = 0);
            _context.ProjectTasks.AddRange(projectTasks);
            _context.SaveChanges();
            return Task.FromResult(_mapper.Map<List<ProjectTaskDto>>(projectTasks));
        }
    }
}
