using CoffeeShopAPI.Data;
using CoffeeShopAPI.Data.dao;
using CoffeeShopAPI.Services.Hash;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHashService, SHA1HashService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

String? connectionString = builder.Configuration.GetConnectionString("MainDb");
MySqlConnection connection = new(connectionString);
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        connection,
        ServerVersion.AutoDetect(connection)
    )
);

builder.Services.AddScoped<UserDao>();
builder.Services.AddScoped<MenuItemDao>();
builder.Services.AddScoped<AdditiveDao>();
builder.Services.AddScoped<CategoryDao>();
builder.Services.AddScoped<AdditiveDao>();
builder.Services.AddScoped<OrderDao>();
builder.Services.AddScoped<OrderItemDao>();

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

app.Run();