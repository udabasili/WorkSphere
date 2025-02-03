using Microsoft.AspNetCore.Mvc;
using WorkSphere.Server.Model;
using WorkSphere.Server.Services;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly ILogger _logger;


        public EmployeesController(EmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;

        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int pageIndex, int pageSize)
        {

            try
            {
                var employees = await _employeeService.PagedEmployeeResponseDto(pageIndex, pageSize);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees");
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                if (id <= 0 | id == null)
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        detail = "ID must be greater than 0",
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                var employee = await _employeeService.GetEmployee(id);
                if (employee == null)
                {
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        detail = $"Employee with ID {id} not found.",
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee");
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

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {

            if (id != employee.Id)
            {
                return BadRequest();
            }
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployee(id, employee);
                if (updatedEmployee == null)
                {
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        detail = $"Employee with ID {id} not found.",
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee employee)
        {

            try
            {

                var newEmployee = await _employeeService.AddEmployee(employee);
                return CreatedAtAction("GetEmployee", new { id = newEmployee.Id }, newEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee");
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

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {

            try
            {
                var employee = await _employeeService.DeleteEmployee(id);
                if (employee == null)
                {
                    return NotFound(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        title = "Not Found",
                        status = 404,
                        detail = $"Employee with ID {id} not found.",
                        traceId = HttpContext.TraceIdentifier
                    });
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
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
