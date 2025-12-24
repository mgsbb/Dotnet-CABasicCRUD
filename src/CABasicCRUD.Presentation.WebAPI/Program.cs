using CABasicCRUD.Application;
using CABasicCRUD.Infrastructure;
using CABasicCRUD.Infrastructure.Persistence;
using CABasicCRUD.Presentation.WebAPI;

var builder = WebApplication.CreateBuilder();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(configuration: builder.Configuration);
builder.Services.RegisterAuthenticationServices();
builder.Services.RegisterPresentationServices(builder.Configuration);

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
