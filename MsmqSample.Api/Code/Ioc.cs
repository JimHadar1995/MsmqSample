using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MsmqSample.Api.Models;
using MsmqSample.Common.Application.Repositories;
using MsmqSample.Common.Application.Services;
using MsmqSample.Common.Infrastructure.Code;
using MsmqSample.Common.Infrastructure.Contexts;
using MsmqSample.Common.Infrastructure.Repositories;
using MsmqSample.Common.Infrastructure.Services;

namespace MsmqSample.Api.Code
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Ioc
    {
        /// <summary>
        /// Initializes the di services.
        /// </summary>
        /// <param name="services">The services.</param>
        internal static void InitializeDiServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            IConfiguration config;
            using (var sp = services.BuildServiceProvider())
            {
                config = sp.GetRequiredService<IConfiguration>();
            }
            // получаем строку подключения из файла конфигурации
            string connection = config.GetConnectionString("DefaultConnection");
            // добавляем контекст в качестве сервиса в приложение
            services.AddDbContext<MsmqSampleDbContext>(options =>
                options.UseSqlServer(connection));
            services.AddSingleton(s => new MsmqProducer(MsmqConsts.MsmqQueueName));

            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true;
                    options.SuppressMapClientErrors = true;
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opt.JsonSerializerOptions.IgnoreNullValues = false;
                    opt.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                });

            services.ConfigureSwagger();
            services.AddHttpContextAccessor();
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            var allowUserSection = config.GetSection(nameof(LocalAllowAuthUser));
            services.Configure<LocalAllowAuthUser>(allowUserSection);
        }

        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API",
                        Version = "v1",
                        Description = "API для работы с задачами",
                        Contact = new OpenApiContact { Name = "test", Email = "test@example.ru", }
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(xmlFile);
                }
            });
        }
    }
}
