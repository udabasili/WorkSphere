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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;


        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;

        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<PagedEmployeeResponseDto>> GetEmployees(int pageIndex, int pageSize)
        {

            try
            {
                var employees = await _employeeService.PagedEmployeeResponseDto(pageIndex, pageSize);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return ErrorHandling.HandleException(ex, HttpContext);

            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
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
                var employee = await _employeeService.GetEmployee(id);
                if (employee == null)
                {
                    errors.Add(new ValidationError(
                        $"Employee with ID {id} not found.",
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
                if (employee.Errors.Count > 0)
                {
                    errors.Add(new ValidationError
                    {
                        Description = "Employee not found",
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
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return ErrorHandling.HandleException(ex, HttpContext);
            }

        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {

            List<ValidationError> errors = new();
            if (id != employee.Id)
            {
                errors.Add(new ValidationError
                {
                    Description = "ID in the URL does not match ID in the body",
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
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployee(id, employee);
                if (updatedEmployee == null)
                {
                    errors.Add(new ValidationError
                    {
                        Description = $"Employee with ID {id} not found.",
                        ErrorType = ErrorType.Model
                    });
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                if (updatedEmployee.Errors.Count > 0)

                {
                    errors.Add(new ValidationError
                    {
                        Description = "Employee not found",
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
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);

            }
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee employee)
        {
            try
            {
                List<ValidationError> errors = new();
                var newEmployee = await _employeeService.AddEmployee(employee);
                if (newEmployee.Errors.Any())
                {
                    errors.Add(new ValidationError
                    {
                        Description = "Employee not found",
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
                return CreatedAtAction("GetEmployee", new { id = newEmployee.Id }, newEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {

            List<ValidationError> errors = new();
            try
            {
                var employee = await _employeeService.DeleteEmployee(id);
                if (employee == null)
                {
                    errors.Add(new ValidationError
                    {
                        Description = $"Employee with ID {id} not found.",
                        ErrorType = ErrorType.Model
                    });
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

    }
}
