using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using ExpenseTracker.AuthApi.Installers;
using ExpenseTracker.AuthApi.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.AuthApi
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
            services.InstallServicesInAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

//            app.UseHealthChecks("/health", new HealthCheckOptions
//            {
//                ResponseWriter = async (context, report) =>
//                {
//                    context.Response.ContentType = "application/json";
//                    var response = new HealthCheckResponse
//                    {
//                        Status = report.Status.ToString(),
//                        Checks = report.Entries.Select(x => new HealthCheck
//                        {
//                            Component = x.Key,
//                            Status = x.Value.Status.ToString(),
//                            Description = x.Value.Description,
//                        }),
//                        Duration = report.TotalDuration,
//                    };
//
//                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
//                }
//            });
//            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseMvc();
        }
    }
}