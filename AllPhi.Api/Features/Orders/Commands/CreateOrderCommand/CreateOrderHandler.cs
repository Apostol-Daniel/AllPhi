using AllPhi.Api.Data;
using AllPhi.Api.Features.Orders.Dtos;
using AllPhi.Api.Models;
using MediatR;

namespace AllPhi.Api.Features.Orders.Commands.CreateOrderCommand;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly AppDbContext _context;

    public CreateOrderHandler(AppDbContext context) => _context = context;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Description = request.Order.Description,
            Price = request.Order.Price,
            CustomerId = request.Order.CustomerId,
            CreationDate = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return new OrderDto(order.Id, order.Description, order.Price, 
            order.CreationDate, order.IsCancelled, order.CustomerId);
    }
}