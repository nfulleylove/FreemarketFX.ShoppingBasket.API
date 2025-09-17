using FreemarketFX.ShoppingBasket.API.Data;
using FreemarketFX.ShoppingBasket.API.Data.Interfaces;
using FreemarketFX.ShoppingBasket.API.Data.Repositories;
using FreemarketFX.ShoppingBasket.API.Models;
using FreemarketFX.ShoppingBasket.API.Services;
using FreemarketFX.ShoppingBasket.API.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<ShoppingDbContext>();

builder.Services.AddScoped<IBasketsRepository, BasketsRepository>();

builder.Services.AddScoped<IBasketService, BasketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    InitialiseDevelopmentDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void InitialiseDevelopmentDatabase()
{
    using IServiceScope scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<ShoppingDbContext>();

    context.Baskets.Add(new()
    {
        Id = new Guid("A1D8D4B0-551D-4A26-A417-5EE3FFE3A4E2"),
        BasketProducts = [
            new (){
                BasketId = new Guid("A1D8D4B0-551D-4A26-A417-5EE3FFE3A4E2"),
                ProductId = new Guid("3CF63300-2F3A-4066-A1D5-47A33D120F36")
            }
            ]
    });

    context.Products.AddRange(
        new Product() { Id = new Guid("{08254E34-D2EF-4065-B646-CB42D0F87EAF}"), Name = "Table", Price = 399.99M },
        new Product() { Id = new Guid("{43AA1547-F36C-480A-A680-2917BAAD4DB1}"), Name = "Chair", Price = 44.99M },
        new Product() { Id = new Guid("3CF63300-2F3A-4066-A1D5-47A33D120F36"), Name = "Lamp", Price = 19.99M, IsDiscounted = true },
        new Product() { Id = new Guid("{A44EDEDD-937A-48BC-BE6A-3F73A3685D44}"), Name = "Book", Price = 5.99M }
    );

    context.SaveChanges();
}