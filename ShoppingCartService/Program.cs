using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Data;
using ShoppingCartService.Services;
using ShoppingCartService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ICartService, CartService>();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Admin", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Admin API", Version = "v1" });
    c.SwaggerDoc("User", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "User API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
        c.SwaggerEndpoint("/swagger/User/swagger.json", "User API");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();