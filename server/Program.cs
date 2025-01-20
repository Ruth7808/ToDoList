using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Service>();
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), 
                     new MySqlServerVersion(new Version(8, 0, 33))));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
app.MapGet("/", () => "hello worls");
app.MapGet("/items", async (Service service) =>
{
    var items = await service.GetItem();
    return items != null ? Results.Ok(items) : Results.NotFound("No items found.");
});

app.MapPost("/items", async ([FromBody] Item item, Service service) =>
{
    var newItem = await service.AddItem(item);
    return newItem != null ? Results.Ok(newItem) : Results.BadRequest("Failed to add item.");
});

app.MapPut("/items/{id}", async (int id, [FromBody] Item item, Service service) =>
{
    var updatedItem = await service.UpdateItem(id, item);
    return updatedItem != null ? Results.Ok(updatedItem) : Results.NotFound("Item not found.");
});

app.MapDelete("/items/{id}", async (int id, Service service) =>
{
    var deletedItem = await service.DeleteItem(id);
    return deletedItem != null ? Results.Ok(deletedItem) : Results.NotFound("Item not found.");
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

