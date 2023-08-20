using Northwind.DataAccess;
using Northwind.UnitOfWork;
using Northwind.WebApi.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Northwind.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IUnitOfWork>(option => new NorthwindUnitOfWork(
    option.GetRequiredService<IConfiguration>().GetConnectionString("Northwind")
));

var tokenProvider = new JwtProvider("issuer", "audience", "northwind_2023");
builder.Services.AddSingleton<ITokenProvider>(tokenProvider);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenProvider.GetValidationParameters();
    });
builder.Services.AddAuthorization(auth =>
{
    auth.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder
            .WithOrigins(corsSettings.AllowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseAuthentication();

app.UseRouting(); // This should come first

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
