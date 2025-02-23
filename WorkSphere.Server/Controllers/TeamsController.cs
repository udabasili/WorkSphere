using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.Entities;
using TastyTreats.Types;
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
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }


        // GET api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTeam(int id)
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
                var response = await _teamService.GetTeamById(id);
                _logger.LogInformation("Team retrieved successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        [HttpPut]
        public async Task<ActionResult<object>> UpdateTeam(UpdateTeamDto updateTeamDto)
        {
            try
            {
                List<ValidationError> errors = new();
                if (updateTeamDto == null)
                {
                    errors.Add(new ValidationError(
                        "Request body cannot be null",
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
                var response = await _teamService.UpdateTeamAsync(updateTeamDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

    }
}
