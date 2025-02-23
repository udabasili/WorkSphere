using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.Entities;
using TastyTreats.Types;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;
using WorkSphere.Server.Services;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly ISalaryService _service;
        private readonly ILogger _logger;

        public SalariesController(ISalaryService service, ILogger<SalariesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/Salaries
        [HttpGet]
        public async Task<ActionResult<PagedSalaryResponseDto>> GetSalaries(int pageIndex, int pageSize)
        {
            try
            {
                _logger.LogInformation("Received request to get salaries");
                var salaries = await _service.GetPagedSalariesAsync(pageIndex, pageSize);
                _logger.LogInformation("Salaries retrieved successfully");
                return Ok(salaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // GET: api/Salaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salary>> GetSalary(int id)
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
                _logger.LogInformation("Received request to get salary with ID: {id}", id);
                var salary = await _service.GetSalaryAsync(id);
                _logger.LogInformation("Salary retrieved successfully");
                return Ok(salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // PUT: api/Salaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalary(int id, Salary salary)
        {

            try
            {
                List<ValidationError> errors = new();
                if (id != salary.Id)
                {
                    errors.Add(new ValidationError
                    {
                        Description = "ID in the URL does not match the ID in the request body",
                        ErrorType = ErrorType.Model
                    });
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                _logger.LogInformation("Received request to update salary with ID: {id}", id);
                var updatedSalary = await _service.UpdateSalaryAsync(id, salary);
                if (updatedSalary == null)
                {
                    errors.Add(new ValidationError
                    {
                        Description = "Salary not found",
                        ErrorType = ErrorType.Model
                    });
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                _logger.LogInformation("Salary updated successfully");
                return Ok(updatedSalary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // POST: api/Salaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Salary>> PostSalary(Salary salary)
        {
            try
            {
                List<ValidationError> errors = new();
                if (salary == null)
                {
                    errors.Add(new ValidationError
                    {
                        Description = "Salary is null",
                        ErrorType = ErrorType.Model
                    });
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                _logger.LogInformation("Received request to create salary");
                var newSalary = await _service.CreateSalaryAsync(salary);
                _logger.LogInformation("Salary created successfully");
                return Ok(newSalary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // DELETE: api/Salaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            try
            {
                List<ValidationError> errors = new();
                if (id <= 0)
                {
                    errors.Add(new ValidationError
                    {
                        Description = "ID must be greater than 0",
                        ErrorType = ErrorType.Model
                    });
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                _logger.LogInformation("Received request to delete salary with ID: {id}", id);
                var salary = await _service.DeleteSalaryAsync(id);
                if (salary == null)
                {
                    errors.Add(new ValidationError
                    {
                        Description = "Salary not found",
                        ErrorType = ErrorType.Model
                    });
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                _logger.LogInformation("Salary deleted successfully");
                return Ok(salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

    }
}
