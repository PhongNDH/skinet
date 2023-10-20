using Api.Errors;
using Api.Extensions;
using Api.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//? Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
//* Using builder.Services.AddApplicationServices(builder.Configuration) to subtitute for
// //? Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// //? Connect to db
// builder.Services.AddDbContext<StoreContext>(otp =>
// {
//     otp.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });
// builder.Services.AddScoped<IProductRespository, ProductRespository>(); // Has Product type
// builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// builder.Services.Configure<ApiBehaviorOptions>(options =>
// {
//     options.InvalidModelStateResponseFactory = actionContext =>
//     {
//         var error = actionContext.ModelState
//             .Where(e => e.Value?.Errors.Count > 0)
//             .SelectMany(x => x.Value!.Errors)
//             .Select(x => x.ErrorMessage)
//             .ToArray();
//         var errorResponse = new ApiValidationErrorResponse { Errors = error };
//         return new BadRequestObjectResult(errorResponse);
//     };
// });

var app = builder.Build();

//? Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

//handle HTTP status code errors
//{0} is a placeholder that will be replaced with the actual HTTP status code. For example, if a 404 error occurs, the middleware will re-execute the route /error/404

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();

// }

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope(); //creates a new scope within the dependency injection container
var services = scope.ServiceProvider; // Resolve and access services registered in the application's dependency injection container
var context = services.GetRequiredService<StoreContext>(); // Resolve an instance of the StoreContext
var logger = services.GetRequiredService<ILogger<Program>>(); // Resolve an instance of the ILogger<Program>
try
{
    await context.Database.MigrateAsync(); //Apply any pending migrations
    await StoreContextSeed.SeedAsync(context); // Seed data in first times
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
