using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WorkSphere.Data;


namespace WorkSphere.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //add the connection string to the services

            var conStrBuilder = new SqlConnectionStringBuilder(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            conStrBuilder.Password = builder.Configuration["Database:password"];
            var connection = conStrBuilder.ConnectionString;

            builder.Services.AddDbContext<WorkSphereDbContext>(options =>
                options
                    .UseSqlServer(connection)
            );


            // Add services to the container.

            builder.Services.AddControllers();

            // Configure Swagger

            var app = builder.Build();


            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}
