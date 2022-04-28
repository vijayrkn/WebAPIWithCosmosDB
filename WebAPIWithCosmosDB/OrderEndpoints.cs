using Microsoft.EntityFrameworkCore;
using WebAPIWithCosmosDB.Data;
using WebAPIWithCosmosDB.Models;
namespace WebAPIWithCosmosDB;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Order", async (OrderContext db) =>
        {
            return await db.Order.ToListAsync();
        })
        .WithName("GetAllOrders")
        .Produces<List<Order>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Order/{id}", async (Guid TrackingId, OrderContext db) =>
        {
            return await db.Order.FindAsync(TrackingId)
                is Order model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetOrderById")
        .Produces<Order>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Order/{id}", async (Guid TrackingId, Order order, OrderContext db) =>
        {
            var foundModel = await db.Order.FindAsync(TrackingId);

            if (foundModel is null)
            {
                return Results.NotFound();
            }
            //update model properties here

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateOrder")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Order/", async (Order order, OrderContext db) =>
        {
            db.Order.Add(order);
            await db.SaveChangesAsync();
            return Results.Created($"/Orders/{order.TrackingId}", order);
        })
        .WithName("CreateOrder")
        .Produces<Order>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Order/{id}", async (Guid TrackingId, OrderContext db) =>
        {
            if (await db.Order.FindAsync(TrackingId) is Order order)
            {
                db.Order.Remove(order);
                await db.SaveChangesAsync();
                return Results.Ok(order);
            }

            return Results.NotFound();
        })
        .WithName("DeleteOrder")
        .Produces<Order>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
