using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Model;
using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Repository.Concrete
{
    public class TeamRepo : ITeamRepo
    {
        private readonly WorkSphereDbContext _context;
        private readonly Random _random = new Random();

        public TeamRepo(WorkSphereDbContext context)
        {
            _context = context;
        }


        public async Task<TeamDto> GetTeamById(int teamId)
        {
            var teams = await GetTeams();
            return teams.FirstOrDefault(team => team.Id == teamId);

        }


        public async Task<PagedTeamResponseDto> GetPagedTeams(int pageIndex, int pageSize)
        {
            var teams = await GetTeams();
            var pagedTeams = teams.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return new PagedTeamResponseDto
            {
                Teams = pagedTeams,
                TotalCount = teams.Count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<TeamDto> UpdateTeamAsync(UpdateTeamDto updateTeamDto)
        {
            var project = await _context.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == updateTeamDto.projectId);
            if (project == null)
            {
                throw new ArgumentException("Project not found.");
            }

            // Get employees added to the team (those in teamMembers but not already in the project)
            //First we use the Except method to get the employees that are in the teamMembers list but not in the project.Employees list.
            //Then we convert the result to a list of ids.
            var employeesAdded = updateTeamDto.teamMembers.Except(project.Employees.Select(e => e.Id)).ToList();

            // Get employees removed from the team (those in the project but not in teamMembers)
            //First we use the Select method to get the ids of the employees in the project.Employees list.
            //Then we use the Except method to get the employees that are in the project.Employees list but not in the teamMembers list.
            var employeesRemoved = project.Employees.Select(e => e.Id).Except(updateTeamDto.teamMembers).ToList();

            // Add new employees to the project
            foreach (var employeeId in employeesAdded)
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
                if (employee != null)
                {
                    project.Employees.Add(employee);

                    // Assign to a task if necessary (you can define the logic to assign tasks based on your requirements)
                    var tasks = await _context.ProjectTasks.Where(t => t.ProjectID == project.Id).ToListAsync();
                    if (tasks.Any())
                    {
                        var randomTask = tasks[_random.Next(tasks.Count)];
                        await AssignEmployeeToTaskAsync(employeeId, randomTask.Id);
                    }
                }
            }

            // Remove employees from the project
            foreach (var employeeId in employeesRemoved)
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
                if (employee != null)
                {
                    project.Employees.Remove(employee);

                    // Remove from all tasks (you can refine this logic based on your needs)
                    var tasksToRemoveEmployeeFrom = await _context.ProjectTasks.Where(t => t.EmployeeID == employeeId && t.ProjectID == project.Id).ToListAsync();
                    foreach (var task in tasksToRemoveEmployeeFrom)
                    {
                        task.EmployeeID = null;
                    }
                }
            }

            // Update project manager if needed
            if (updateTeamDto.projectManagerId != 0 && project.ProjectManagerID != updateTeamDto.projectManagerId)
            {
                var projectManager = await _context.ProjectManagers.FirstOrDefaultAsync(m => m.Id == updateTeamDto.projectManagerId);
                if (projectManager != null)
                {
                    project.ProjectManager = projectManager;
                }
            }

            await _context.SaveChangesAsync();
            return await GetTeamById(updateTeamDto.projectId);
        }


        private async Task<List<TeamDto>> GetTeams()
        {
            var projects = await _context.Projects.Include(p => p.ProjectManager).ToListAsync();
            var teams = new List<TeamDto>();

            foreach (var project in projects)
            {
                var teamMembers = await GetTeamMembers(project.Id);
                var numOfCompletedTasks = await _context.ProjectTasks.CountAsync(t => t.ProjectID == project.Id && t.Status == Status.Completed);
                var numOfPendingTasks = await _context.ProjectTasks.CountAsync(t => t.ProjectID == project.Id && t.Status != Status.Completed);

                teams.Add(new TeamDto
                {
                    Id = project.Id,
                    ProjectName = project.Name,
                    ProjectManager = project.ProjectManager,
                    TeamMembers = teamMembers,
                    NumOfCompletedTasks = numOfCompletedTasks,
                    NumOfPendingTasks = numOfPendingTasks
                });
            }

            return teams;
        }

        private async Task<List<TeamMemberDto>> GetTeamMembers(int projectId)
        {
            var taskIds = await _context.ProjectTasks.Where(t => t.ProjectID == projectId).Select(t => t.Id).ToListAsync();

            var teamMembers = await _context.Employees
                .Where(e => e.ProjectTasks.Any(t => taskIds.Contains(t.Id)))
                .Select(e => new TeamMemberDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                })
                .ToListAsync();

            return teamMembers;
        }

        private async Task<bool> AssignEmployeeToTaskAsync(int employeeId, int taskId)
        {
            var task = await _context.ProjectTasks.FirstOrDefaultAsync(task => task.Id == taskId);
            if (task == null)
            {
                return false;
            }

            task.EmployeeID = employeeId;
            await _context.SaveChangesAsync();
            return true;
        }



    }

    //

}