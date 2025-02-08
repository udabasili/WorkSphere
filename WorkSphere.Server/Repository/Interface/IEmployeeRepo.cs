using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository
{
    public interface IEmployeeRepo
    {

        Task<Employee> GetEmployeeAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee employee);

        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);

        Task<Employee> DeleteEmployeeAsync(int id);

        Task<PagedEmployeeResponseDto> GetPagedEmployeesAsync(int pageIndex, int pageSize);

        Task<bool> EmployeeEmailExists(Employee employee);


    }
}
