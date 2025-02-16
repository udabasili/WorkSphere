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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Received request to update project tasks for ProjectID: {ProjectID}", projectID);
                _logger.LogInformation("Tasks: {Tasks}", JsonConvert.SerializeObject(updateDto.Tasks));

                var existingTasks = await _service.GetProjectTasksAsync(projectID);
                var existingTaskIds = existingTasks.ProjectTasks.Select(t => t.Id).ToHashSet();

                List<ProjectTask> tasksToUpdate = new List<ProjectTask>();
                List<ProjectTask> tasksToAdd = new List<ProjectTask>();

                foreach (var task in updateDto.Tasks)
                {
                    foreach (var employee in task.EmployeeIDs)
                    {
                        if (existingTaskIds.Contains(task.Id))
                        {
                            var existingTask = await _context.ProjectTasks
                                .AsNoTracking()
                                .FirstOrDefaultAsync(t => t.Id == task.Id && t.EmployeeID == employee);
                            //remove the old employees from the task
                            // Remove old employee-task assignments

                            if (existingTask != null)
                            {
                                existingTask.EmployeeID = null;
                                tasksToUpdate.Add(existingTask);
                            }


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

                var response = await _service.BulkUpdateProjectTasks(projectTaskBulkInsertDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project tasks for ProjectID: {ProjectID}", projectID);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception: {InnerException}", ex.InnerException.Message);
                }

                return StatusCode(500, new { message = "An error occurred while updating the project tasks.", error = ex.Message });
            }
        }








        //// POST: api/ProjectTasks
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<ProjectTask>> PostProjectTask(ProjectTask projectTask)
        //{
        //    _context.ProjectTasks.Add(projectTask);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProjectTask", new { id = projectTask.Id }, projectTask);
        //}

        //// DELETE: api/ProjectTasks/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProjectTask(int id)
        //{
        //    var projectTask = await _context.ProjectTasks.FindAsync(id);
        //    if (projectTask == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ProjectTasks.Remove(projectTask);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ProjectTaskExists(int id)
        //{
        //    return _context.ProjectTasks.Any(e => e.Id == id);
        //}
    }
}
