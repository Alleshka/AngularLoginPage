using AngularLoginPage.Context;
using Microsoft.EntityFrameworkCore;

namespace AngularLoginPage.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AngularLoginPageDbContext>(options =>
            {
                options.UseSqlite("Data Source=Database.db");
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseCors("AllowFrontend");

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
