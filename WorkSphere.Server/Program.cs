using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;
using WorkSphere.Server.Data;
using WorkSphere.Server.Model;


namespace WorkSphere.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
                conStrBuilder.Password = builder.Configuration["Database:password"];
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

            builder.Services.AddControllers();

            // Configure Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}
