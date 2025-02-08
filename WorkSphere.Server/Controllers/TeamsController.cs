using Microsoft.AspNetCore.Mvc;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger _logger;
        public TeamsController(ITeamService teamService, ILogger<TeamsController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }
        // GET: api/Teams?pageIndex=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTeams(int pageIndex, int pageSize)
        {
            try
            {
                var response = await _teamService.GetPagedTeams(pageIndex, pageSize);
                _logger.LogInformation("Teams retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting teams");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    detail = ex.Message,
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }


        // GET api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTeam(int id)
        {
            try
            {
                var response = await _teamService.GetTeamById(id);
                _logger.LogInformation("Team retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting team");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    detail = ex.Message,
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<object>> UpdateTeam(UpdateTeamDto updateTeamDto)
        {
            try
            {

                var response = await _teamService.UpdateTeamAsync(updateTeamDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting team");
                return StatusCode(500, new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    detail = ex.Message,
                    traceId = HttpContext.TraceIdentifier
                });
            }
        }

    }
}
