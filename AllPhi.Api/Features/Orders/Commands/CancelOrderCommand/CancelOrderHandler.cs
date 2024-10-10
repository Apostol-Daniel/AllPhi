using AllPhi.Api.Data;
using AllPhi.Api.Features.Orders.Dtos;
using AllPhi.Api.Middleware.Exceptions;
using MediatR;

namespace AllPhi.Api.Features.Orders.Commands.CancelOrderCommand;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, OrderDto>
{
    private readonly AppDbContext _context;

    public CancelOrderHandler(AppDbContext context) => _context = context;

    public async Task<OrderDto> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
        
        if (order == null)
            throw new NotFoundException($"Order with ID {request.OrderId} not found.");

        order.IsCancelled = true;
        await _context.SaveChangesAsync(cancellationToken);

        return new OrderDto(order.Id, order.Description, order.Price, 
            order.CreationDate, order.IsCancelled, order.CustomerId);
    }
}