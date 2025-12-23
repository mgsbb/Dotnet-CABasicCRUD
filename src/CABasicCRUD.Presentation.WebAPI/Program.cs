using CABasicCRUD.Application;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Infrastructure;
using CABasicCRUD.Infrastructure.Authentication;
using CABasicCRUD.Infrastructure.Persistence;
using CABasicCRUD.Presentation.WebAPI.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(configuration: builder.Configuration);
builder.Services.RegisterAuthenticationServices();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

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
