using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        public readonly WorkSphereDbContext _context;

        public EmployeeRepo(WorkSphereDbContext context)
        {
            _context = context;
        }

        private async Task<List<EmployeeDto>> GetEmployees(int pageIndex = 0, int pageSize = 0)
        {
            if (pageSize == 0)
            {
                return await _context.Employees.Select(employee => new EmployeeDto
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Salary = employee.Salary,
                }).ToListAsync();
            }

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

        private async Task<int> GetEmployeesCount()
        {
            return await _context.Employees.CountAsync();
        }

        public async Task<PagedEmployeeResponseDto> GetPagedEmployeesAsync(int pageIndex, int pageSize)
        {
            var employees = await GetEmployees(pageIndex, pageSize);
            var totalCount = await GetEmployeesCount();
            return new PagedEmployeeResponseDto
            {
                Employees = employees,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            var employee = await _context.Employees
                .Include(employee => employee.Salary)
                .FirstOrDefaultAsync(employee => employee.Id == id);
            return employee;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }


        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;

        }


        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> EmployeeEmailExists(Employee employee)
        {

            return await _context.Employees.AnyAsync(e => e.Email == employee.Email && e.Id != employee.Id);
        }

    }
}
