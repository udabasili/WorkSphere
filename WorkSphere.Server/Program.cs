using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
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
                                      policy.WithOrigins("http://127.0.0.1:4200",
                                                         "http://localhost:4200",
                                                         "https://work-sphere-app.netlify.app") // Remove trailing slash
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials(); // Allow cookies, authentication headers, etc.
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


            builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
            builder.Services.AddScoped<IProjectTaskRepo, ProjectTaskRepo>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddScoped<ITeamRepo, TeamRepo>();
            builder.Services.AddScoped<IProjectManagerRepo, ProjectManagerRepo>();
            builder.Services.AddScoped<ISalaryRepo, SalaryRepo>();
            builder.Services.AddScoped<ILoginRepo, LoginRepo>();

            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IProjectManagerService, ProjectManagerService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ISalaryService, SalaryService>();
            builder.Services.AddScoped<ILoginService, LoginService>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                string? jwtKey = builder.Configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("Jwt:Key is not configured.");
                }

                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Configure authorization policies.
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ProjectManager", policy =>
                {
                    policy.RequireRole("ProjectManager");
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Employee", policy =>
                {
                    policy.RequireRole("Employee");
                });
            });

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
