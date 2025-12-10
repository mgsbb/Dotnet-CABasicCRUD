using CABasicCRUD.Application;
using CABasicCRUD.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(configuration: builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
