using FreemarketFX.ShoppingBasket.API.Data;
using FreemarketFX.ShoppingBasket.API.Data.Interfaces;
using FreemarketFX.ShoppingBasket.API.Data.Repositories;
using FreemarketFX.ShoppingBasket.API.Services;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ShoppingDbContext>();

builder.Services.AddScoped<IBasketsRepository, BasketsRepository>();

builder.Services.AddScoped<IBasketService, BasketService>();

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