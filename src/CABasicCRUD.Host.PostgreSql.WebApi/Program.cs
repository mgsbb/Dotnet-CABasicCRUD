using CABasicCRUD.Application;
using CABasicCRUD.Infrastructure;
using CABasicCRUD.Infrastructure.Persistence.PostgreSql;
using CABasicCRUD.Presentation.WebApi;

var builder = WebApplication.CreateBuilder();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterAuthenticationServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterPresentationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
