using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagersController : ControllerBase
    {
        private readonly WorkSphereDbContext _context;

        public ProjectManagersController(WorkSphereDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectManagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectManager>>> GetProjectManagers(int pageIndex, int pageSize)
        {
            return await _context.ProjectManagers
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // GET: api/ProjectManagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManager>> GetProjectManager(int id)
        {
            var projectManager = await _context.ProjectManagers.FindAsync(id);

            if (projectManager == null)
            {
                return NotFound();
            }

            return projectManager;
        }

        // PUT: api/ProjectManagers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectManager(int id, ProjectManager projectManager)
        {
            if (id != projectManager.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectManager).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectManagerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectManagers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectManager>> PostProjectManager(ProjectManager projectManager)
        {
            _context.ProjectManagers.Add(projectManager);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectManager", new { id = projectManager.Id }, projectManager);
        }

        // DELETE: api/ProjectManagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectManager(int id)
        {
            var projectManager = await _context.ProjectManagers.FindAsync(id);
            if (projectManager == null)
            {
                return NotFound();
            }

            _context.ProjectManagers.Remove(projectManager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectManagerExists(int id)
        {
            return _context.ProjectManagers.Any(e => e.Id == id);
        }
    }
}
