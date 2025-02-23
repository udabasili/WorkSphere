using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Services
{
    public interface ISalaryService
    {
        public Task<PagedSalaryResponseDto> GetPagedSalariesAsync(int pageIndex, int pageSize);
        public Task<Salary> GetSalaryAsync(int salaryId);
        public Task<Salary> CreateSalaryAsync(Salary salary);
        public Task<Salary> UpdateSalaryAsync(int? id, Salary salary);
        public Task<Salary> DeleteSalaryAsync(int salaryId);
    }
}
