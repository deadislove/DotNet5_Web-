using DotNet5_web.API.Logging.Interface;
using DotNet5_web.API.Logging.Repository;
using DotNet5_web.Core.Domain.Interface;
using DotNet5_web.Core.Domain.Services;
using DotNet5_web.Core.Interface;
using DotNet5_web.Core.Services;
using DotNet5_web.Infra.DBContext;
using DotNet5_web.Infra.DBContext.Interface;
using DotNet5_web.Infra.DBContext.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet5_web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Database connection setting.
            services.AddDbContext<DemoDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DB_EntityString"));
            }, ServiceLifetime.Transient);

            // AOP
            services.AddScoped(typeof(ILogRepository), typeof(LogRepository));

            // Basic DI services
            services.AddScoped(typeof(ICRUD<>), typeof(CrudServices<>));
            services.AddScoped(typeof(IGenericTypeRepository<>), typeof(GenericTypeRepository<>));
            //
            services.AddScoped(typeof(IDomain), typeof(DomainServices));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNet5_web.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNet5_web.API v1"));
            }

            //Add log file path
            var logPath = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{logPath}\\Logs\\Log.txt");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
