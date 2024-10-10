using AllPhi.Api.Data;
using AllPhi.Api.Features.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Orders.Queries.GetCustomerOrdersQuery;

public class GetCustomerOrdersHandler : IRequestHandler<GetCustomerOrdersQuery, List<OrderDto>>
{
    private readonly AppDbContext _context;

    public GetCustomerOrdersHandler(AppDbContext context) => _context = context;

    public async Task<List<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == request.CustomerId)
            .Select(o => new OrderDto(o.Id, o.Description, o.Price, 
                o.CreationDate, o.IsCancelled, o.CustomerId))
            .ToListAsync(cancellationToken);
    }
}