using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using System.Diagnostics;

internal class Program
{
   
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        builder.Services.AddOpenTelemetry()
             .WithTracing(
            builder =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddHttpClientInstrumentation(); 
                builder.AddSource("*");
                builder.AddConsoleExporter();
            }
            );

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        
        builder.Services.AddLogging(loggingbuilder =>
        {
          
            loggingbuilder.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
                options.AddConsoleExporter();
              
            });
        });
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        
        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}