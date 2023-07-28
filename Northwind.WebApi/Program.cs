using Northwind.DataAccess;
using Northwind.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IUnitOfWork>(option => new NorthwindUnitOfWork(
    option.GetRequiredService<IConfiguration>().GetConnectionString("Northwind")
));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // This should come first

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
