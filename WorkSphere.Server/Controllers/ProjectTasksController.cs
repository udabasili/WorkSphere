using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
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

        public ProjectTasksController(IProjectTaskService service, ILogger<ProjectTasksController> logger)
        {
            _service = service;
            _logger = logger;

        }

        // GET: api/ProjectTasks?projectID
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProjectTasks(int projectID)
        {
            var response = await _service.GetProjectTasksAsync(projectID);
            if (response == null || !response.ProjectTasks.Any())
                return NotFound("No tasks found for this project.");

            return Ok(response);
        }



        //// GET: api/ProjectTasks/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProjectTask>> GetProjectTask(int id)
        //{
        //    var projectTask = await _context.ProjectTasks.FindAsync(id);

        //    if (projectTask == null)
        //    {
        //        return NotFound();
        //    }

        //    return projectTask;
        //}

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
                // ✅ Use ILogger for logging
                _logger.LogInformation("Received request to update project tasks for ProjectID: {ProjectID}", projectID);
                _logger.LogInformation("Tasks: {Tasks}", JsonConvert.SerializeObject(updateDto.Tasks));

                var projectTaskBulkInsertDto = new ProjectTaskBulkInsertDto
                {
                    ProjectId = projectID,
                    Tasks = updateDto.Tasks.Select(task => new ProjectTask
                    {
                        Id = task.Id,
                        Name = task.Name,
                        Description = task.Description,
                        ProjectID = task.ProjectID,
                        EmployeeID = task.EmployeeID,
                        Order = task.Order,
                        Status = Enum.Parse<Status>(task.Status) // Might throw an error
                    }).ToList()
                };


                var response = await _service.BulkUpdateProjectTasks(projectTaskBulkInsertDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project tasks for ProjectID: {ProjectID}", projectID);

                // ✅ Log inner exception details
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
