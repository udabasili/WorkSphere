using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Model;

namespace WorkSphere.Server.Repository
{
    public class SalaryRepo : ISalaryRepo
    {
        public readonly WorkSphereDbContext _context;

        public SalaryRepo(WorkSphereDbContext context)
        {
            _context = context;
        }

        public async Task<PagedSalaryResponseDto> GetPagedSalariesAsync(int pageIndex = 0, int pageSize = 10)
        {
            var salaries = new List<Salary>();
            if (pageSize <= 0)
            {
                salaries = await _context.Salaries
               .Include(salary => salary.ProjectManager)
               .Include(salary => salary.Employee)
               .OrderBy(salary => salary.Employee != null ? salary.Employee.FirstName : salary.ProjectManager.FirstName)
               .ToListAsync();
                return new PagedSalaryResponseDto
                {
                    Salaries = salaries,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = salaries.Count
                };
            }

            salaries = await _context.Salaries
               .Include(project => project.ProjectManager)
               .Include(project => project.Employee)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalProjects = await GetTotalSalariesCountAsync();

            return new PagedSalaryResponseDto
            {
                Salaries = salaries,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalProjects
            };
        }

        private async Task<int> GetTotalSalariesCountAsync()
        {
            return await _context.Salaries.CountAsync();
        }

        public async Task<Salary> GetSalaryAsync(int salaryId)
        {
            if (salaryId == 0)
            {
                return null;
            }
            return await _context.Salaries
                .Include(salary => salary.ProjectManager)
                .Include(salary => salary.Employee)
                .FirstOrDefaultAsync(salary => salary.Id == salaryId);
        }


        public async Task<Salary> CreateSalaryAsync(Salary salary)
        {
            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();
            return salary;
        }

        public async Task<Salary> DeleteSalaryAsync(int salaryId)
        {
            var salary = await _context.Salaries.FindAsync(salaryId);
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return salary;
        }

        public async Task<Salary> UpdateSalaryAsync(int? id, Salary salary)
        {
            if (id != salary.Id)
            {
                return null;
            }
            _context.Entry(salary).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return salary;
        }

        private bool SalaryExists(int? id)
        {
            return _context.Salaries.Any(e => e.Id == id);
        }
    }

}