using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MsmqSample.Api.Code;
using MsmqSample.Api.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace MsmqSample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.InitializeDiServices();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();            

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });

            app.Use(async (m, next) =>
            {
                var allowUserOptions = m.RequestServices.GetRequiredService<IOptions<LocalAllowAuthUser>>();
                var authUser = m.Request.HttpContext.User;
                if ((allowUserOptions.Value == null ||
                    !authUser.Identity.IsAuthenticated ||
                    allowUserOptions.Value.User.ToLower() != authUser.Identity.Name!.ToLower()))
                {
                    m.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await m.Response.WriteAsync("Forbidden");
                    return;
                }
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapFallbackToController("index", "home");
                });
            });
        }
    }
}
