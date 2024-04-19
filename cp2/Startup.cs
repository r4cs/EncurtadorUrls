using cp2.HealthChecks;
using cp2.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace cp2;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }  
    
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddControllers();
    
        services.AddHttpContextAccessor(); // !!

        services.AddDbContext<EncurtadorDbContext>(options =>
        {
            options.UseOracle(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<EncurtadorUrlService>();
        services.AddScoped<ApiService>();
        // services.AddScoped<HealthChecksDb>();
    
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Encurtador de links v1", Version = "v1" });

        });
        
        services.AddHealthChecks()
            .AddOracle(Configuration.GetConnectionString("DefaultConnection") ?? string.Empty,
                healthQuery: "SELECT 1 FROM DUAL", name: "Oracle Fiap Server",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "Feedback", "Database" })
            .AddCheck<RemoteHealthCheck>("Remote Health Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Remote" });
        
        
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        
        // services.AddHealthChecksUI(opt =>
        // {
        //     opt.SetEvaluationTimeInSeconds(10);
        //     opt.MaximumHistoryEntriesPerEndpoint(60);
        //     opt.SetApiMaxActiveRequests(1);
        //     opt.AddHealthCheckEndpoint("API Health", "/api/health-ui");
        // });
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Encurtador de urls V1");
            c.RoutePrefix = string.Empty;
        });

        app.UseRouting();

        app.UseHttpsRedirection();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/api/health", new HealthCheckOptions()
            {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
        
        // app.UseHealthChecksUI(options =>
        // {
        // options.UIPath = "/api/health-ui";
        // });
    }
}