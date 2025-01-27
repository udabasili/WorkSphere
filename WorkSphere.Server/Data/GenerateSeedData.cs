namespace WorkSphere.Server.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WorkSphere.Data;
using WorkSphere.Model;
using WorkSphere.Server.Enums;
using WorkSphere.Server.Model;

public class SeedData
{
    private static Random _random = new Random();

    // Sample first names and last names
    private static readonly List<string> _firstNames = new List<string>
        {
            "John", "Jane", "Michael", "Sarah", "David", "Emma", "James", "Olivia", "Daniel", "Sophia"
        };

    private static readonly List<string> _lastNames = new List<string>
        {
            "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Lee", "Wilson", "Davis", "Miller", "Moore"
        };

    // Predefined lists of adjectives, nouns, and industry terms
    private static readonly List<string> _adjectives = new List<string>
        {
            "Innovative", "Dynamic", "Advanced", "Strategic", "Global", "NextGen", "Optimized", "Efficient", "Revolutionary", "Smart"
        };

    private static readonly List<string> _nouns = new List<string>
        {
            "Platform", "Solution", "Network", "System", "Engine", "Application", "Service", "Framework", "Module", "Interface"
        };

    private static readonly List<string> _industries = new List<string>
        {
            "Tech", "Finance", "Health", "Logistics", "Marketing", "AI", "Cloud", "Blockchain", "Ecommerce", "Cybersecurity"
        };

    // Method to generate a random project name
    private static string GenerateProjectName()
    {
        // Randomly select an adjective, noun, and industry term
        var adjective = _adjectives[_random.Next(_adjectives.Count)];
        var noun = _nouns[_random.Next(_nouns.Count)];
        var industry = _industries[_random.Next(_industries.Count)];

        // Combine them to form a project name (e.g., "Innovative Blockchain Solution")
        return $"{adjective} {industry} {noun}";
    }

    // Generate random first and last name
    private static string GenerateFullName()
    {
        var firstName = _firstNames[_random.Next(_firstNames.Count)];
        var lastName = _lastNames[_random.Next(_lastNames.Count)];
        return $"{firstName} {lastName}";
    }

    public static class TaskNameGenerator
    {
        private static Random _random = new Random();

        // Predefined lists of action verbs, objectives, and contexts
        private static readonly List<string> _actionVerbs = new List<string>
        {
            "Design", "Develop", "Test", "Review", "Implement", "Analyze", "Optimize", "Configure", "Document", "Deploy"
        };

        private static readonly List<string> _objectives = new List<string>
        {
            "User Interface", "Backend System", "Database Schema", "API Integration", "Performance Metrics",
            "Bug Fixes", "Security Protocols", "Feature Updates", "Codebase", "System Architecture"
        };

        private static readonly List<string> _contexts = new List<string>
        {
            "for Client Dashboard", "for Mobile App", "for Reporting Module", "for Data Processing",
            "for Authentication System", "for Project Management", "for Billing System",
            "for Analytics Dashboard", "for File Storage", "for Cloud Deployment"
        };

        // Method to generate a random task name
        public static string GenerateTaskName()
        {
            // Randomly select an action verb, objective, and context
            var actionVerb = _actionVerbs[_random.Next(_actionVerbs.Count)];
            var objective = _objectives[_random.Next(_objectives.Count)];
            var context = _contexts[_random.Next(_contexts.Count)];

            // Combine them to form a task name (e.g., "Develop API Integration for Mobile App")
            return $"{actionVerb} {objective} {context}";


        }
    }



    // Generate a list of employees
    public static List<Employee> GetEmployees(int count = 30)
    {
        var employees = new List<Employee>();

        for (int i = 0; i < count; i++)
        {
            var fullName = GenerateFullName();
            employees.Add(new Employee
            {
                FirstName = fullName.Split(' ')[0],
                LastName = fullName.Split(' ')[1],
                Email = $"{fullName.Split(' ')[0].ToLower()}.{fullName.Split(' ')[1].ToLower()}@example.com",
                EmploymentDate = DateTime.Now.AddDays(-_random.Next(365, 1825)), // Random employment date within the last 5 years
            });
        }

        return employees;
    }

    // Generate a list of projects
    public static List<Project> GetProjects(int count = 30)
    {
        var projects = new List<Project>();

        for (int i = 0; i < count; i++)
        {
            projects.Add(new Project
            {
                Name = GenerateProjectName(),
                Description = $"Description of Project {i + 1}",
                StartDate = DateTime.Now.AddDays(-_random.Next(1, 200)),
                EndDate = DateTime.Now.AddDays(_random.Next(200, 400)),
            });
        }

        return projects;
    }

    // Generate a list of project managers
    public static List<ProjectManager> GetProjectManagers(int count = 30)
    {
        var projectManagers = new List<ProjectManager>();

        for (int i = 0; i < count; i++)
        {
            var fullName = GenerateFullName();
            projectManagers.Add(new ProjectManager
            {
                FirstName = fullName.Split(' ')[0],
                LastName = fullName.Split(' ')[1],
                Email = $"{fullName.Split(' ')[0].ToLower()}.{fullName.Split(' ')[1].ToLower()}@example.com",
                EmploymentDate = DateTime.Now.AddDays(-_random.Next(365, 1825)), // Random employment date within the last 5 years
            });
        }

        return projectManagers;
    }

    // Generate a list of tasks
    public static List<ProjectTask> GetTasks(int count = 15)
    {
        var tasks = new List<ProjectTask>();

        for (int i = 0; i < count; i++)
        {
            tasks.Add(new ProjectTask
            {
                Name = TaskNameGenerator.GenerateTaskName(),
                Description = $"Description of Task {i + 1}",
                Status = (Status)_random.Next(3), // Random status

            });
        }

        return tasks;
    }

    // Generate a list of salaries
    public static List<Salary> GetSalaries(int count = 30)
    {
        var salaries = new List<Salary>();

        for (int i = 0; i < count; i++)
        {
            salaries.Add(new Salary
            {
                BasicSalary = _random.Next(30000, 100000), // Random basic salary between 30k and 100k
                Bonus = _random.Next(1000, 5000), // Random bonus between 1k and 5k
                Deductions = _random.Next(500, 2000), // Random deductions between 500 and 2k
            });
        }

        return salaries;
    }

    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "admin@test.com",
            Email = "admin@test.com",
            EmailConfirmed = true,
            Role = Role.Admin.ToString()
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            // Create user with password
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Password123!");
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Employee.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enums.Roles.ProjectManager.ToString());
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
        // Assign project managers to projects
        var projects = context.Projects.ToList();
        var projectMans = context.ProjectManagers.ToList();
        Dictionary<int, List<int>> projectManagerProjects = new Dictionary<int, List<int>>();
        foreach (var projecMan in projectMans)
        {
            projectManagerProjects.Add(projecMan.Id, new List<int>());

        }

        foreach (var project in projects)
        {
            // Randomly assign a project manager to each project
            var projectManagerId = projectMans[_random.Next(projectMans.Count)].Id;
            // Set the ProjectManagerID property of the project
            project.ProjectManagerID = projectManagerId;
            // Add the project to the project manager's list of projects
            projectManagerProjects[projectManagerId].Add(project.Id);
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedTaskAsync(WorkSphereDbContext context)
    {
        var tasks = GetTasks(); // Static list of tasks (common to all projects)

        if (!context.ProjectTasks.Any())
        {
            var projects = context.Projects.ToList();
            var employees = context.Employees.ToList();

            foreach (var project in projects)
            {
                // Ensure each employee is only assigned one task per project
                var assignedEmployeeIds = new HashSet<int>();

                for (int i = 0; i < tasks.Count; i++)
                {
                    // Assign task to an employee not already assigned in this project
                    var availableEmployee = employees.FirstOrDefault(e => !assignedEmployeeIds.Contains(e.Id));
                    if (availableEmployee == null)
                    {
                        throw new InvalidOperationException("Not enough employees to assign tasks for the project.");
                    }

                    // Clone the task object to avoid modifying the original task list
                    var projectTask = new ProjectTask
                    {
                        Name = tasks[i].Name,
                        Description = tasks[i].Description,
                        Order = i + 1,
                        ProjectID = project.Id,
                        EmployeeID = availableEmployee.Id
                    };

                    assignedEmployeeIds.Add(availableEmployee.Id);
                    context.ProjectTasks.Add(projectTask);
                }
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
                salary.EmployeeID = employees.Where(e => e.SalaryID == salary.Id).FirstOrDefault().Id;
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
        await SeedUsersAsync(userManager, context);

        await SeedTaskAsync(context);
        await SeedSalaryAsync(context);
    }

}
