﻿

namespace WorkSphere.Server.Data;

using Bogus;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSphere.Data;
using WorkSphere.Model;
using WorkSphere.Server.Enums;
using WorkSphere.Server.Model;

public class SeedData
{
    private static Random _random = new Random();
    private static Faker _faker = new Faker();

    // Method to generate a random project name
    private static string GenerateProjectName()
    {
        return _faker.Company.CatchPhrase();
    }

    public static List<Employee> GetEmployees(int count = 30)
    {
        return new Faker<Employee>()
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Email, (f, e) => $"{e.FirstName.ToLower()}.{e.LastName.ToLower()}@example.com")
            .RuleFor(e => e.EmploymentDate, f => f.Date.Past(5))
            .Generate(count);
    }

    public static List<Project> GetProjects(int count = 30)
    {
        return new Faker<Project>()
            .RuleFor(p => p.Name, f => GenerateProjectName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.StartDate, f => f.Date.Past(1))
            .RuleFor(p => p.Status, f => f.PickRandom<Status>())
            .Generate(count);
    }

    public static List<ProjectManager> GetProjectManagers(int count = 30)
    {
        return new Faker<ProjectManager>()
            .RuleFor(pm => pm.FirstName, f => f.Name.FirstName())
            .RuleFor(pm => pm.LastName, f => f.Name.LastName())
            .RuleFor(pm => pm.Email, (f, pm) => $"{pm.FirstName.ToLower()}.{pm.LastName.ToLower()}@example.com")
            .RuleFor(pm => pm.EmploymentDate, f => f.Date.Past(5))
            .Generate(count);
    }

    public static List<ProjectTask> GetTasks(int count = 15)
    {
        return new Faker<ProjectTask>()
            .RuleFor(t => t.Name, f => f.Hacker.Verb() + " " + f.Hacker.Noun())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence())
            .RuleFor(t => t.Status, f => f.PickRandom<Status>())
            .RuleFor(t => t.Duration, f => f.Random.Int(1, 7))
            .Generate(count);
    }

    public static List<Salary> GetSalaries(int count = 60)
    {
        return new Faker<Salary>()
            .RuleFor(s => s.BasicSalary, f => f.Random.Decimal(30000, 100000))
            .RuleFor(s => s.Bonus, f => f.Random.Decimal(1000, 5000))
            .RuleFor(s => s.Deductions, f => f.Random.Decimal(500, 2000))
            .Generate(count);
    }

    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "admin@test.com",
            Email = "admin@test.com",
            EmailConfirmed = true,
            Role = Enums.Roles.Admin.ToString() // Set the Role property
        };

        // Check if the Admin role exists, and create it if not
        if (!await roleManager.RoleExistsAsync(Enums.Roles.Admin.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
        }

        // Ensure the user does not already exist before creating
        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
            var createUserResult = await userManager.CreateAsync(defaultUser, "Password123!");
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
            }
        }
    }




    public static async Task SeedEmployeeAsync(WorkSphereDbContext context)
    {
        if (!context.Employees.Any())
        {
            var employees = GetEmployees();
            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedProjectAsync(WorkSphereDbContext context)
    {
        if (!context.Projects.Any())
        {
            var projects = GetProjects();
            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedProjectManagerAsync(WorkSphereDbContext context)
    {
        if (!context.ProjectManagers.Any())
        {
            var projectManagers = GetProjectManagers();
            context.ProjectManagers.AddRange(projectManagers);
            await context.SaveChangesAsync();
        }

        var projects = context.Projects.ToList();
        var projectManagersList = context.ProjectManagers.ToList();

        foreach (var project in projects)
        {
            var projectManagerId = projectManagersList[_random.Next(projectManagersList.Count)].Id;
            project.ProjectManagerID = projectManagerId;
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedTaskAsync(WorkSphereDbContext context)
    {
        if (!context.ProjectTasks.Any())
        {
            var tasks = GetTasks();
            var projects = context.Projects.ToList();
            var employees = context.Employees.ToList();

            for (int proj = 0; proj < projects.Count; proj++)
            {
                var project = projects[proj];
                var assignedEmployeeIds = new HashSet<int>();

                for (int i = 0; i < tasks.Count; i++)
                {
                    var availableEmployee = employees.FirstOrDefault(e => !assignedEmployeeIds.Contains(e.Id));
                    if (availableEmployee == null)
                    {
                        throw new InvalidOperationException("Not enough employees to assign tasks for the project.");
                    }

                    var projectTask = new ProjectTask
                    {
                        Name = tasks[i].Name,
                        Description = tasks[i].Description,
                        Duration = tasks[i].Duration,
                        Status = tasks[i].Status,
                        ProjectID = project.Id,
                        EmployeeID = availableEmployee.Id,
                        //increment the order by 1 for each project 
                        Order = i + 1
                    };

                    assignedEmployeeIds.Add(availableEmployee.Id);
                    context.ProjectTasks.Add(projectTask);
                }
            }

            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedProjectManagerSalaryAsyc(WorkSphereDbContext context)
    {
        var projectManagers = context.ProjectManagers.ToList();

        // Assign salaries to project managers
        var salaries = await context.Salaries.ToListAsync();
        //get salaries without employee id i.e salary with no employee attached
        var salariesWithoutEmployee = salaries.Where(s => s.EmployeeID == 0 || s.EmployeeID == null).ToList();

        for (int i = 0; i < projectManagers.Count; i++)
        {
            projectManagers[i].SalaryID = salariesWithoutEmployee[i].Id;
        }
        await context.SaveChangesAsync();
        //set the project manager id to the salary
        //get salaries without employee id
        foreach (var salary in salariesWithoutEmployee)
        {
            var projectManager = projectManagers.Where(e => e.SalaryID == salary.Id).FirstOrDefault();
            if (projectManager != null)
            {
                salary.ProjectManagerID = projectManager.Id;
            }
            else
            {
                // Handle the case where no matching ProjectManager is found
                // For example, log a warning or throw an exception
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedSalaryAsync(WorkSphereDbContext context)
    {
        if (!context.Salaries.Any())
        {
            var employees = context.Employees.ToList();
            List<Salary> salaries = GetSalaries();
            context.Salaries.AddRange(salaries);
            await context.SaveChangesAsync();

            // Assign salaries to employees

            salaries = await context.Salaries.ToListAsync();
            for (int i = 0; i < employees.Count; i++)
            {
                employees[i].SalaryID = salaries[i].Id;
            }
            await context.SaveChangesAsync();
            //set the employee id to the salary
            foreach (var salary in salaries)
            {
                var employee = employees.Where(e => e.SalaryID == salary.Id).FirstOrDefault();
                if (employee != null)
                {
                    salary.EmployeeID = employee.Id;
                }
            }


            await context.SaveChangesAsync();
        }
    }


    public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, WorkSphereDbContext context)
    {
        string defaultPassword = "Password123!";

        // Create project manager users
        var projectManagers = await context.ProjectManagers.ToListAsync();
        foreach (var projectManager in projectManagers)
        {
            var email = $"{projectManager.FirstName.ToLower()}.{projectManager.LastName.ToLower()}@example.com";
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                var projectManagerUser = new ProjectManagerUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    Role = Roles.ProjectManager.ToString(),
                    ProjectManagerId = projectManager.Id // Link to the ProjectManager record

                };

                var result = await userManager.CreateAsync(projectManagerUser, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(projectManagerUser, Roles.ProjectManager.ToString());
                }

            }




        }

        // Create employee users
        var employees = await context.Employees.ToListAsync();
        foreach (var employee in employees)
        {
            var email = employee.Email;
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                var employeeUser = new EmployeeUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    Role = Roles.Employee.ToString(),
                    EmployeeID = employee.Id // Link to the Employee record

                };

                var result = await userManager.CreateAsync(employeeUser, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, Roles.Employee.ToString());
                }
            }
        }

        // Save any changes to the agent records
        await context.SaveChangesAsync();

    }


    public static async Task SeedDataAsync(WorkSphereDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedAdminAsync(userManager, roleManager);
        await SeedEmployeeAsync(context);
        await SeedProjectAsync(context);
        await SeedProjectManagerAsync(context);
        await SeedTaskAsync(context);
        await SeedSalaryAsync(context);
        await SeedProjectManagerSalaryAsyc(context);
        await SeedUsersAsync(userManager, context);

    }
}
