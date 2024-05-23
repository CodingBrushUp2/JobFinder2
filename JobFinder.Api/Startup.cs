using FluentValidation.AspNetCore;
using JobFinder.Api.Middleware;
using JobFinder.Domain.Models.Contracts;
using JobFinder.Domain.Models.DTOs;
using JobFinder.Api.Services;
using JobFinder.Api.Validators;
using Microsoft.OpenApi.Models;
using JobFinder.Domain.Services;

namespace JobFinder.Api;
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));
        services.AddSingleton<IAuthService, AuthService>();
        services.AddTransient<IJobService, JobService>();

        services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApplicantValidator>());

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Job API", Version = "v1" });
        });

        services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job API v1"));
        }

        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
