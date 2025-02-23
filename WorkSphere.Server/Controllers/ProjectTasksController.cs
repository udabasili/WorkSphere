using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkSphere.Data;
using WorkSphere.Model;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;
using WorkSphere.Server.Services;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTasksController : ControllerBase
    {
        private readonly IProjectTaskService _service;
        private readonly ILogger<ProjectTasksController> _logger;
        private readonly WorkSphereDbContext _context;

        public ProjectTasksController(IProjectTaskService service, ILogger<ProjectTasksController> logger, WorkSphereDbContext context)
        {
            _service = service;
            _logger = logger;
            _context = context;

        }

        // GET: api/ProjectTasks?projectID
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProjectTasks(int projectID)
        {
            var response = await _service.GetProjectTasksAsync(projectID);
            if (response == null || !response.ProjectTasks.Any())
                response = new ProjectTasksResponseDto
                {
                    ProjectTasks = new List<ProjectTaskDto>(),
                    ProjectTeamMembers = new List<TeamMemberDto>()
                };

            return Ok(response);
        }


        // PUT: api/ProjectTasks
        //the input is an array of project tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProjectTask(int projectID, [FromBody] UpdateProjectTasksDto updateDto)
        {
            try
            {
                ProjectTaskBulkInsertDto projectTaskBulkInsertDto = await ProjectTaskUpdateHandler(projectID, updateDto);

                var response = await _service.BulkUpdateProjectTasks(projectTaskBulkInsertDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        /// <summary>
        ///  This method is responsible for updating the project tasks
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        private async Task<ProjectTaskBulkInsertDto> ProjectTaskUpdateHandler(int projectID, UpdateProjectTasksDto updateDto)
        {
            _logger.LogInformation("Received request to update project tasks for ProjectID: {ProjectID}", projectID);
            _logger.LogInformation("Tasks: {Tasks}", JsonConvert.SerializeObject(updateDto.Tasks));

            // Get existing tasks for the project
            var existingTasks = await _service.GetProjectTasksAsync(projectID);
            // Get the existing task IDs as a HashSet for quick lookup
            var existingTaskIds = existingTasks.ProjectTasks.Select(t => t.Id).ToHashSet();

            List<ProjectTask> tasksToUpdate = new List<ProjectTask>();
            List<ProjectTask> tasksToAdd = new List<ProjectTask>();

            // Loop through the tasks in the update DTO
            foreach (var task in updateDto.Tasks)
            {
                // Loop through the employees assigned to the task
                foreach (var employee in task.EmployeeIDs)
                {
                    if (existingTaskIds.Contains(task.Id))
                    {
                        // Get the existing task for the employee and we use asnno tracking to avoid conflicts
                        var existingTask = await _context.ProjectTasks
                            .AsNoTracking()
                            .FirstOrDefaultAsync(t => t.Id == task.Id && t.EmployeeID == employee);

                        // First we set employee to null for the task. This is to avoid conflicts when updating the task

                        if (existingTask != null)
                        {
                            existingTask.EmployeeID = null;
                            tasksToUpdate.Add(existingTask);
                        }

                        // if the task exists for the employee, we update the task and reset the employee
                        if (existingTask != null)
                        {
                            existingTask.Name = task.Name;
                            existingTask.Description = task.Description;
                            existingTask.ProjectID = task.ProjectID;
                            existingTask.EmployeeID = employee;
                            existingTask.Order = task.Order;
                            existingTask.Status = Enum.Parse<Status>(task.Status);
                            existingTask.Duration = task.Duration;
                            tasksToUpdate.Add(existingTask);
                        }
                        // If the task does not exist for the employee, we add it to the list of tasks to add
                        else
                        {
                            tasksToAdd.Add(new ProjectTask
                            {
                                Id = task.Id,
                                Name = task.Name,
                                Description = task.Description,
                                ProjectID = task.ProjectID,
                                EmployeeID = employee,
                                Order = task.Order,
                                Status = Enum.Parse<Status>(task.Status),
                                Duration = task.Duration
                            });
                        }
                    }
                    else
                    {
                        tasksToAdd.Add(new ProjectTask
                        {
                            Id = task.Id,
                            Name = task.Name,
                            Description = task.Description,
                            ProjectID = task.ProjectID,
                            EmployeeID = employee,
                            Order = task.Order,
                            Status = Enum.Parse<Status>(task.Status),
                            Duration = task.Duration
                        });
                    }
                }
            }

            _context.ChangeTracker.Clear(); // Clear the change tracker to avoid tracking conflicts

            _context.ProjectTasks.UpdateRange(tasksToUpdate);
            //_context.ProjectTasks.AddRange(tasksToAdd);

            await _context.SaveChangesAsync();

            var projectTaskBulkInsertDto = new ProjectTaskBulkInsertDto
            {
                ProjectId = projectID,
                Tasks = tasksToAdd
            };
            return projectTaskBulkInsertDto;
        }



    }
}
