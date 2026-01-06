using CABasicCRUD.Application;
using CABasicCRUD.Infrastructure;
using CABasicCRUD.Infrastructure.Persistence.Sqlite;
using CABasicCRUD.Presentation.WebApi;
using CABasicCRUD.Presentation.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder();

builder.Services.RegisterApplicationServices();

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);

builder.Services.RegisterPresentationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
