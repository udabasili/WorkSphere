using Microsoft.EntityFrameworkCore;
using WorkSphere.Model;

namespace WorkSphere.Data
{
    public class WorkSphereDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public WorkSphereDbContext(DbContextOptions<WorkSphereDbContext> options) : base(options)
        {
        }
    }
}
