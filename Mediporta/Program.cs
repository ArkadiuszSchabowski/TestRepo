using Mediporta.Database;
using Mediporta.Middleware;
using Mediporta.Seeders;
using Mediporta.Services;
using Mediporta.Validators;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

namespace Mediporta
{
    public class Program
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static async Task Main(string[] args)
        {
            _logger.Info("App started");
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ITagSeeder, TagSeeder>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ITagValidator, TagRequestValidator>();
            builder.Logging.AddNLog();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MediportaConnectionString")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mediporta API v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<MyDbContext>();

                dbContext.Database.Migrate();

                if (!dbContext.Tags.Any())
                {
                    var service = services.GetRequiredService<ITagService>();
                    var seeder = services.GetRequiredService<ITagSeeder>();
                    var tags = await service.ReloadTasks();
                    seeder.SaveTagsToDatabase(tags);
                }
            }
            app.Run();
        }
    }
}