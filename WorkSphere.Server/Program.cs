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

            try
            {
                using (var con = new SqlConnection(connection))
                {
                    con.Open();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
