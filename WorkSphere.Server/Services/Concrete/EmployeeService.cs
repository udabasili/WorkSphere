using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    public class EmployeeService : IEmployeeService
    {

        public readonly IEmployeeRepo _employeeRepo;

        public EmployeeService(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<PagedEmployeeResponseDto> PagedEmployeeResponseDto(int pageIndex, int pageSize)
        {
            return await _employeeRepo.PagedEmployeeResponseDto(pageIndex, pageSize);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            return await _employeeRepo.AddEmployee(employee);
        }

        public async Task<Employee> DeleteEmployee(int id)
        {
            return await _employeeRepo.DeleteEmployee(id);
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _employeeRepo.GetEmployee(id);
        }

        public async Task<Employee> UpdateEmployee(int id, Employee employee)
        {
            return await _employeeRepo.UpdateEmployee(employee);
        }


    }
}
