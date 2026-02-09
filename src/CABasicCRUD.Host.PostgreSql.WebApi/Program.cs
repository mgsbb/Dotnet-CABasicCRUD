using CABasicCRUD.Application;
using CABasicCRUD.Infrastructure;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql.Seeding;
using CABasicCRUD.Presentation.WebApi;
using CABasicCRUD.Presentation.WebApi.Middlewares;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

var seedOptions = app.Configuration.GetSection("Database").Get<DatabaseSeedOptions>();

if (seedOptions?.IsSeedDatabase == true)
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<RawSqlSeeder>();

    await seeder.SeedAsync();
}

app.MapGet("/", () => "Hello World!");

app.Run();
