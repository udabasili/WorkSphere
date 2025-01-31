using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly WorkSphereDbContext _context;
        private readonly ILogger _logger;


        public EmployeesController(WorkSphereDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int pageIndex, int pageSize)
        {
            /**
             * Response Object
             * {
                    employees: Employee[],
                    pageIndex: number,
                    pageSize: number,
                    totalCount: number
                  }
             * **/
            var employees = await _context.Employees.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await _context.Employees.CountAsync();

            return Ok(new { employees, totalCount, pageIndex, pageSize });
        }


        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(employee => employee.Salary)
                .FirstOrDefaultAsync(employee => employee.Id == id);

            if (employee == null)
            {
                return NotFound();
            }


            return employee;
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

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                if (EmployeeEmailExists(employee.Email))
                {
                    return BadRequest("Email already exists");
                }

                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Invalid employee data");
            }

            if (EmployeeEmailExists(employee.Email))
            {
                return BadRequest("Email already exists");
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        //check if employee email exists
        private bool EmployeeEmailExists(string email)
        {
            return _context.Employees.Any(e => e.Email == email);
        }
    }
}
