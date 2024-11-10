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

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseAuthorization();

            app.UseEndpoints(config =>
            {
                config.MapControllers();
                config.MapFallbackToController("Index", "Fallback");
            });
            
            app.Run();
        }
    }
}
