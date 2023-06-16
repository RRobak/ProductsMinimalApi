using Microsoft.EntityFrameworkCore;
using OrdersMinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OrderDb>(opt => opt.UseInMemoryDatabase("OrderList"));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapPost("/orders", async (Order order, OrderDb db) =>
{
    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/orders/{order.Id}", order);
});

app.MapGet("/orders", async (OrderDb db) =>
    await db.Orders.ToListAsync());

app.MapGet("/orders/{status}", async (string status, OrderDb db) =>
{
    var orders = await db.Orders.Where(o => o.Status == status).ToListAsync();
    return Results.Ok(orders);
});

app.MapPut("/orders/{id}/status", async (int id, string newStatus, OrderDb db) =>
{
    var order = await db.Orders.FindAsync(id);

    if (order is null) return Results.NotFound();

    order.Status = newStatus;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();