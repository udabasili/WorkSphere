using Microsoft.AspNetCore.Mvc;
using WorkSphere.Model;
using WorkSphere.Server.Services;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects(int pageIndex = 0, int pageSize = 10)
        {

            try
            {
                var projects = await _projectService.GetPagedProjectsAsync(pageIndex, pageSize);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var project = await _projectService.GetProjectAsync(id);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, [FromBody] Project project)
        {

            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var updatedProject = await _projectService.UpdateProjectAsync(id, project);
                return Ok(updatedProject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject([FromBody] Project project)
        {

            try
            {
                var newProject = await _projectService.CreateProjectAsync(project);
                return Ok(newProject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                _projectService.DeleteProjectAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
    }

}
