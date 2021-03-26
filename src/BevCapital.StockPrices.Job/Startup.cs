using BevCapital.StockPrices.Infra.ServiceExtensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Text.Json;

namespace BevCapital.StockPrices.Job
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                    .ConfigureCommonServices(Configuration)
                    .ConfigureSecurity()
                    .ConfigureDistributedCache(Configuration)
                    .ConfigureAWS(Configuration, HostingEnvironment)
                    .ConfigureDatabase(Configuration)
                    .ConfigureHttpClients(Configuration)
                    .ConfigureBackgroundServices()
                    .ConfigureHealthCheck(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information($"Hosting enviroment = {env.EnvironmentName}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAWS();
            app.UseSecurity();
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = reg => reg.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.Map("/", async (context) =>
                {
                    var result = JsonSerializer.Serialize(new
                    {
                        machineName = Environment.MachineName,
                        appName = env.ApplicationName,
                        environment = env.EnvironmentName
                    });

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result.ToString());
                });
            });

        }
    }
}
