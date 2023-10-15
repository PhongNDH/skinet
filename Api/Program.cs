using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Connect to db
builder.Services.AddDbContext<StoreContext>(otp =>
{
    otp.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRespository, ProductRespository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope(); //creates a new scope within the dependency injection container
var services = scope.ServiceProvider; // Resolve and access services registered in the application's dependency injection container
var context = services.GetRequiredService<StoreContext>(); // Resolve an instance of the StoreContext
var logger = services.GetRequiredService<ILogger<Program>>(); // Resolve an instance of the ILogger<Program>
try
{
    await context.Database.MigrateAsync(); //A pply any pending migrations
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
