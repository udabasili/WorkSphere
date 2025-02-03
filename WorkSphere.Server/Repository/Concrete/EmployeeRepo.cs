using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository.Concrete
{
    public class EmployeeRepo : IEmployeeRepo
    {
        public readonly WorkSphereDbContext _context;

        public EmployeeRepo(WorkSphereDbContext context)
        {
            _context = context;
        }

        private async Task<List<EmployeeDto>> GetEmployees(int pageIndex = 0, int pageSize = 10)
        {
            var employees = await _context.Employees.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            return employees.Select(employee => new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Salary = employee.Salary,
            }).ToList();
        }

        private int GetEmployeesCount()
        {
            return _context.Employees.Count();
        }

        public Task<PagedEmployeeResponseDto> PagedEmployeeResponseDto(int pageIndex, int pageSize)
        {
            var employees = GetEmployees(pageIndex, pageSize);
            var totalCount = GetEmployeesCount();
            return Task.FromResult(new PagedEmployeeResponseDto
            {
                Employees = employees.Result,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(employee => employee.Salary)
                .FirstOrDefaultAsync(employee => employee.Id == id);
            return employee;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            if (EmployeeEmailExists(employee.Email))
            {
                throw new Exception("Email already exists");
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }


        public async Task<Employee> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                throw new Exception("Id mismatch");
            }

            if (EmployeeEmailExists(employee.Email))
            {
                throw new Exception("Email already exists");
            }

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;

        }


        public async Task<Employee> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        private bool EmployeeEmailExists(string email)
        {
            return _context.Employees.Any(e => e.Email == email);
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

    }
}
