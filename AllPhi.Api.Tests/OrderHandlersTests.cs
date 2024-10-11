using AllPhi.Api.Data;
using AllPhi.Api.Data.Models;
using AllPhi.Api.Features.Orders.Commands.CreateOrderCommand;
using AllPhi.Api.Features.Orders.Dtos;
using AllPhi.Api.Features.Orders.Queries.GetCustomerOrdersQuery;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Tests;

public class OrderHandlersTests
{
    private readonly AppDbContext _context;
    private readonly CreateOrderHandler _createOrderHandler;
    private readonly GetCustomerOrdersHandler _getCustomerOrdersHandler;

    public OrderHandlersTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _createOrderHandler = new CreateOrderHandler(_context);
        _getCustomerOrdersHandler = new GetCustomerOrdersHandler(_context);
    }

    [Fact]
    public async Task CreateOrder_ShouldCreateNewOrder()
    {
        // Arrange
        var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var createOrderDto = new CreateOrderDto("Test Order", 100m, customer.Id);
        var command = new CreateOrderCommand(createOrderDto);

        // Act
        var result = await _createOrderHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createOrderDto.Description, result.Description);
        Assert.Equal(createOrderDto.Price, result.Price);
        Assert.Equal(customer.Id, result.CustomerId);
        Assert.False(result.IsCancelled);
    }

    [Fact]
    public async Task GetCustomerOrders_ShouldReturnCustomerOrders()
    {
        // Arrange
        var customer = new Customer { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var orders = new List<Order>
        {
            new() { CustomerId = customer.Id, Description = "Order 1", Price = 100m },
            new() { CustomerId = customer.Id, Description = "Order 2", Price = 200m }
        };
        _context.Orders.AddRange(orders);
        await _context.SaveChangesAsync();

        var query = new GetCustomerOrdersQuery(customer.Id);

        // Act
        var result = await _getCustomerOrdersHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, order => Assert.Equal(customer.Id, order.CustomerId));
    }
}