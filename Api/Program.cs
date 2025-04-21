using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Concrete;
using Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnectionString")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

builder.Services.AddEndpointsApiExplorer(); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:30754")
              .AllowAnyHeader()
              .AllowAnyMethod());
});



var app = builder.Build();

 
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
