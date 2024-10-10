using AllPhi.Api.Data;
using AllPhi.Api.Features.Orders.Dtos;
using AllPhi.Api.Models;
using MediatR;

namespace AllPhi.Api.Features.Orders.Commands.CreateOrderCommand;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(AppDbContext context, ILogger<CreateOrderHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new order for customer {CustomerId}", request.Order.CustomerId);
        
        var order = new Order
        {
            Description = request.Order.Description,
            Price = request.Order.Price,
            CustomerId = request.Order.CustomerId,
            CreationDate = DateTime.UtcNow
        };

        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Successfully created order {OrderId} for customer {CustomerId}", 
                order.Id, order.CustomerId);

            return new OrderDto(order.Id, order.Description, order.Price, 
                order.CreationDate, order.IsCancelled, order.CustomerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for customer {CustomerId}", request.Order.CustomerId);
            throw;
        }
    }
}