using Microsoft.EntityFrameworkCore;

namespace WorkSphere.Data
{
    public class WorkSphereDbContext : DbContext
    {
        public WorkSphereDbContext(DbContextOptions<WorkSphereDbContext> options) : base(options)
        {
        }
    }
}
