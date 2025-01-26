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

            // Configure relationships and any other custom settings

            // Employee ↔ Salary One-to-One relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Salary)
                .WithOne(s => s.Employee)
                .HasForeignKey<Employee>(e => e.SalaryID);

            // Project ↔ ProjectManager One-to-Many relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(pm => pm.ManagedProjects)
                .HasForeignKey(p => p.ProjectManagerID)
                .OnDelete(DeleteBehavior.SetNull); // Prevent cascading delete of project when project manager is deleted

            // ProjectTask ↔ Project One-to-Many relationship
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(t => t.ProjectID)
                .OnDelete(DeleteBehavior.Cascade); // If a project is deleted, its tasks will be deleted as well

            // ProjectTask ↔ Employee One-to-Many relationship
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.ProjectTasks)
                .HasForeignKey(t => t.EmployeeID)
                .OnDelete(DeleteBehavior.SetNull); // Allow tasks to remain even if an employee is deleted

            // ProjectManager ↔ ProjectManagerUser One-to-One relationship
            modelBuilder.Entity<ProjectManagerUser>()
                .HasOne(pm => pm.ProjectManager)
                .WithMany()
                .HasForeignKey(pm => pm.ManagerID)
                .OnDelete(DeleteBehavior.SetNull); // Allow project manager user to exist without a project manager

            // EmployeeUser ↔ Employee One-to-One relationship
            modelBuilder.Entity<EmployeeUser>()
                .HasOne(eu => eu.Employee)
                .WithMany()
                .HasForeignKey(eu => eu.EmployeeID)
                .OnDelete(DeleteBehavior.SetNull); // Allow employee user to exist without an employee entity

            // Add additional configurations if needed
        }
    }
}
