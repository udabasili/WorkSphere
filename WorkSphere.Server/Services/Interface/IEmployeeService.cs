using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Services
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployee(int id);

        Task<Employee> AddEmployee(Employee employee);

        Task<Employee> UpdateEmployee(int id, Employee employee);

        Task<Employee> DeleteEmployee(int id);

        Task<PagedEmployeeResponseDto> PagedEmployeeResponseDto(int pageIndex, int pageSize);
    }
}
