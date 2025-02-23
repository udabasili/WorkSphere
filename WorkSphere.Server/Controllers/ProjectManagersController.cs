using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.Entities;
using TastyTreats.Types;
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
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // GET: api/ProjectManagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManager>> GetProjectManager(int id)
        {
            try
            {
                List<ValidationError> errors = new();

                if (id <= 0)
                {
                    errors.Add(new ValidationError(
                        "ID must be greater than 0",
                        ErrorType.Model
                    ));
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var projectManager = await _projectManagerService.GetProjectManager(id);
                if (projectManager == null)
                {
                    errors.Add(new ValidationError(
                        $"ProjectManager with ID {id} not found",
                        ErrorType.Model
                    ));
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(projectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // PUT: api/ProjectManagers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectManager(int id, ProjectManager projectManager)
        {

            try
            {
                List<ValidationError> errors = new();

                if (id != projectManager.Id)
                {
                    errors.Add(new ValidationError(
                        "ID mismatch",
                        ErrorType.Model
                    ));
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var updatedProjectManager = await _projectManagerService.UpdateProjectManager(id, projectManager);
                if (updatedProjectManager.Errors.Count > 0)
                {
                    errors.Add(new ValidationError(
                        "ProjectManager not found",
                        ErrorType.Model
                    ));
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(updatedProjectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // POST: api/ProjectManagers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectManager>> PostProjectManager(ProjectManager projectManager)
        {

            try
            {
                List<ValidationError> errors = new();
                if (projectManager == null)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var newProjectManager = await _projectManagerService.AddProjectManager(projectManager);
                if (newProjectManager.Errors.Count > 0)
                {
                    errors.Add(new ValidationError(
                        "ProjectManager not found",
                        ErrorType.Model
                    ));
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return CreatedAtAction("GetProjectManager", new { id = newProjectManager.Id }, newProjectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // DELETE: api/ProjectManagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectManager(int id)
        {

            try
            {
                List<ValidationError> errors = new();
                var projectManager = await _projectManagerService.DeleteProjectManager(id);
                if (projectManager == null)
                {
                    errors.Add(new ValidationError(
                        $"ProjectManager with ID {id} not found",
                        ErrorType.Model
                    ));
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(projectManager);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }


    }
}
