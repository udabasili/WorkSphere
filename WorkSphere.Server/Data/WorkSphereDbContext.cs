using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Model;
using WorkSphere.Server.Model;

namespace WorkSphere.Data
{
    public class WorkSphereDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Salary> Salaries { get; set; }

        public WorkSphereDbContext(DbContextOptions<WorkSphereDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee ↔ Salary One-to-One relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Salary)
                .WithOne(s => s.Employee)
                .HasForeignKey<Salary>(s => s.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);

            // ProjectManager ↔ Salary One-to-One relationship
            modelBuilder.Entity<ProjectManager>()
                .HasOne(pm => pm.Salary)
                .WithOne(s => s.ProjectManager)
                .HasForeignKey<Salary>(s => s.ProjectManagerID)
                .OnDelete(DeleteBehavior.Cascade); // Ensure Salary gets deleted when ProjectManager is deleted


            // Project ↔ ProjectManager One-to-Many relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(pm => pm.ManagedProjects)
                .HasForeignKey(p => p.ProjectManagerID)
                .OnDelete(DeleteBehavior.SetNull); // Prevent project deletion when project manager is removed

            // ProjectTask ↔ Project One-to-Many relationship
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(t => t.ProjectID)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a project deletes all associated tasks

            // ProjectTask ↔ Employee One-to-Many relationship
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.ProjectTasks)
                .HasForeignKey(t => t.EmployeeID)
                .OnDelete(DeleteBehavior.SetNull); // Tasks remain even if employee is removed

            // ProjectManagerUser ↔ ProjectManager One-to-One relationship
            modelBuilder.Entity<ProjectManagerUser>()
                .HasOne(pm => pm.ProjectManager)
                .WithOne()
                .HasForeignKey<ProjectManagerUser>(pm => pm.ProjectManagerId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a project manager deletes their user account

            // EmployeeUser ↔ Employee One-to-One relationship
            modelBuilder.Entity<EmployeeUser>()
                .HasOne(eu => eu.Employee)
                .WithOne()
                .HasForeignKey<EmployeeUser>(eu => eu.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade); // Deleting an employee deletes their user account

        }
    }
}
