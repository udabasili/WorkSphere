using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;
using WorkSphere.Data;
using WorkSphere.Server.Data;
using WorkSphere.Server.Model;
using WorkSphere.Server.Repository;
using WorkSphere.Server.Repository.Concrete;
using WorkSphere.Server.Services;


namespace WorkSphere.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console() // Fix: Add using Serilog.Sinks.Console;
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://127.0.0.1:4200", "http://localhost:4200", "https://work-sphere-app.netlify.app/", "https://work-sphere-app.netlify.app/") // Explicitly allow the frontend origin
                                            .AllowAnyHeader() // Allow any headers
                                            .AllowAnyMethod(); // Allow any HTTP methods (GET, POST, etc.)
                                  });
            });

            //add the connection string to the services

            //if environment is development, use the user secrets
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<WorkSphereDbContext>(options =>
                    options
                        .UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"))
                );
            }
            else
            {
                var conStrBuilder = new SqlConnectionStringBuilder(
                builder.Configuration.GetConnectionString("ProductionConnection"));
                // conStrBuilder.Password = builder.Configuration["Database:password"];
                var connection = conStrBuilder.ConnectionString;

                builder.Services.AddDbContext<WorkSphereDbContext>(options =>
                    options
                        .UseSqlServer(connection)
                );
            }




            //add authentication services
            builder.Services.AddAuthorization();


            /**
             * Identity Configuration
             */
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Stores.MaxLengthForKeys = 128;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WorkSphereDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityCore<EmployeeUser>().AddEntityFrameworkStores<WorkSphereDbContext>();
            builder.Services.AddIdentityCore<ProjectManagerUser>().AddEntityFrameworkStores<WorkSphereDbContext>();

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    //we need to add this ignore cyclic error
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                });


            //// Add services to the container.

            //builder.Services.AddControllers();

            // Configure Swagger only

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            //Add DI  
            // Register Repositories
            builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
            builder.Services.AddScoped<IProjectTaskRepo, ProjectTaskRepo>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddScoped<ITeamRepo, TeamRepo>();
            builder.Services.AddScoped<IProjectManagerRepo, ProjectManagerRepo>();
            builder.Services.AddScoped<ISalaryRepo, SalaryRepo>();

            // Register Services
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IProjectManagerService, ProjectManagerService>();
            builder.Services.AddScoped<ISalaryService, SalaryService>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            // Seed DB
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<WorkSphereDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await ContextSeed.SeedRoleAsync(userManager, roleManager);
                    await SeedData.SeedDataAsync(context, userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured seeding the DB.");
                }
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI();



            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);




            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}
