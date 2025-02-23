using AutoMapper;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepo _repository;
        private readonly IMapper _mapper;

        public SalaryService(ISalaryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedSalaryResponseDto> GetPagedSalariesAsync(int pageIndex, int pageSize)
        {
            return await _repository.GetPagedSalariesAsync(pageIndex, pageSize);
        }


        public async Task<Salary> GetSalaryAsync(int salaryId)
        {
            return await _repository.GetSalaryAsync(salaryId);
        }

        public async Task<Salary> CreateSalaryAsync(Salary salary)
        {
            return await _repository.CreateSalaryAsync(salary);
        }

        public async Task<Salary> UpdateSalaryAsync(int? id, Salary salary)
        {
            return await _repository.UpdateSalaryAsync(id, salary);
        }

        public async Task<Salary> DeleteSalaryAsync(int salaryId)
        {
            return await _repository.DeleteSalaryAsync(salaryId);
        }

    }
}
