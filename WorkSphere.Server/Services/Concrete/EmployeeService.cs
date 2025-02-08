using System.ComponentModel.DataAnnotations;
using TastyTreats.Types;
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
            return await _employeeRepo.GetPagedEmployeesAsync(pageIndex, pageSize);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            if (await Validate(employee))
            {
                return await _employeeRepo.AddEmployeeAsync(employee);
            }
            return employee;
        }

        public async Task<Employee> DeleteEmployee(int id)
        {
            return await _employeeRepo.DeleteEmployeeAsync(id);
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _employeeRepo.GetEmployeeAsync(id);
        }

        public async Task<Employee> UpdateEmployee(int id, Employee employee)
        {
            if (await Validate(employee))
            {
                return await _employeeRepo.UpdateEmployeeAsync(id, employee);
            }
            return employee;
        }

        //validation

        private async Task<bool> Validate(Employee employee)
        {
            await ValidateEmail(employee);
            ValidateModel(employee);

            return employee.Errors.Count == 0;
        }

        private async Task<bool> ValidateEmail(Employee employee)
        {
            if (await _employeeRepo.EmployeeEmailExists(employee))
            {

                employee.AddError("Email already exists", ErrorType.Business);
                return false;
            }
            return true;

        }

        private void ValidateModel(Employee employee)
        {
            List<ValidationResult> results = new();
            Validator.TryValidateObject(employee, new ValidationContext(employee), results, true);

            foreach (ValidationResult e in results)
                employee.AddError(e.ErrorMessage, ErrorType.Model);
        }

    }
}


