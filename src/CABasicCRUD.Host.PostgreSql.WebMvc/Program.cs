using CABasicCRUD.Application;
using CABasicCRUD.Infrastructure;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql.Seeding;
using CABasicCRUD.Presentation.WebMvc;
// using Microsoft.Extensions.FileProviders;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder();

builder.Services.RegisterApplicationServices();

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);

builder.Services.RegisterPresentationServices();

builder.Host.UseSerilog(
    (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
);

builder
    .Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddOtlpExporter();
    });

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapStaticAssets();

// app.UseStaticFiles();

/*
For static files in Presentation.WebMvc to work, assembly name needs to be exposed. For e.g:
<link rel="stylesheet" href="/_content/CABasicCRUD.Presentation.WebMvc/lib/bootstrap/dist/css/bootstrap.min.css">
The above will work for dotnet run or dotnet publish output.
Publish directory contains wwwroot from Presentation.WebMvc at this path exposing assembly name:
publish/wwwroot/_content/CABasicCRUD.Presentation.WebMvc

To use without exposing assembly name, i.e for e.g:
<link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css">,
explicit file provider can be used as below.
But the below code won't work for the published output, which docker also requires.

Moving wwwroot into host project seems to be the best option for now.
*/

// try
// {
//     app.UseStaticFiles(
//         new StaticFileOptions
//         {
//             FileProvider = new PhysicalFileProvider(
//                 Path.Combine(
//                     builder.Environment.ContentRootPath,
//                     "../CABasicCRUD.Presentation.WebMvc/wwwroot"
//                 )
//             ),
//             RequestPath = "",
//         }
//     );
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex);
// }

var seedOptions = app.Configuration.GetSection("Database").Get<DatabaseSeedOptions>();

if (seedOptions?.IsSeedDatabase == true)
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<RawSqlSeeder>();

    await seeder.SeedAsync();
}

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
