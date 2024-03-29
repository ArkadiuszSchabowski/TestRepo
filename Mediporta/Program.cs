using Mediporta.Database;
using Mediporta.Middleware;
using Mediporta.Seeders;
using Mediporta.Services;
using Mediporta.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System;

namespace Mediporta
{
    public class Program
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
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
            builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TestDatabaseConnectionString")));

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
                var tagSeeder = services.GetRequiredService<ITagSeeder>();
                tagSeeder.SeedTagsToDatabase();
                }
            }
            app.Run();
        }
    }
}