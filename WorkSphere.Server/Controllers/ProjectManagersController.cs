using Microsoft.AspNetCore.Mvc;
using WorkSphere.Server.Model;
using WorkSphere.Server.Services;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagersController : ControllerBase
    {
        private readonly IProjectManagerService _projectManagerService;
        private readonly ILogger _logger;


        public ProjectManagersController(IProjectManagerService projectManagerService, ILogger<ProjectManagersController> logger)
        {
            _projectManagerService = projectManagerService;
            _logger = logger;

        }
        // GET: api/ProjectManagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectManager>>> GetProjectManagers(int pageIndex, int pageSize)
        {
            try
            {
                var projectManagers = await _projectManagerService.PagedProjectManagerResponseDto(pageIndex, pageSize);
                return Ok(projectManagers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting projectManagers");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ProjectManagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManager>> GetProjectManager(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors = new { id = new[] { "ID must be greater than 0" } },
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var projectManager = await _projectManagerService.GetProjectManager(id);
                if (projectManager == null)
                {
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        errors = new { id = new[] { $"ProjectManager with ID {id} not found." } },
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(projectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting projectManager");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    errors = new { message = new[] { ex.Message } },
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }

        // PUT: api/ProjectManagers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectManager(int id, ProjectManager projectManager)
        {

            try
            {
                if (id != projectManager.Id)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors = new { id = new[] { "ID mismatch" } },
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var updatedProjectManager = await _projectManagerService.UpdateProjectManager(id, projectManager);
                if (updatedProjectManager.Errors.Count > 0)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors = updatedProjectManager.Errors.ToDictionary(e => e.ErrorType.ToString(), e => new[] { e.Description }),
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(updatedProjectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating projectManager");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    errors = new { message = new[] { ex.Message } },
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }

        // POST: api/ProjectManagers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectManager>> PostProjectManager(ProjectManager projectManager)
        {

            try
            {
                if (projectManager == null)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors = new { id = new[] { "ProjectManager is null" } },
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var newProjectManager = await _projectManagerService.AddProjectManager(projectManager);
                if (newProjectManager.Errors.Count > 0)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors = newProjectManager.Errors.ToDictionary(e => e.ErrorType.ToString(), e => new[] { e.Description }),
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return CreatedAtAction("GetProjectManager", new { id = newProjectManager.Id }, newProjectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding projectManager");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    errors = new { message = new[] { ex.Message } },
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }

        // DELETE: api/ProjectManagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectManager(int id)
        {

            try
            {
                var projectManager = await _projectManagerService.DeleteProjectManager(id);
                if (projectManager == null)
                {
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        errors = new { id = new[] { $"ProjectManager with ID {id} not found." } },
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(projectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting projectManager");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    errors = new { message = new[] { ex.Message } },
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }


    }
}
