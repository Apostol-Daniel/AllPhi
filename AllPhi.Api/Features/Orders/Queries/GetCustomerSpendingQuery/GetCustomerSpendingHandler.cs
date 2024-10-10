using AllPhi.Api.Data;
using AllPhi.Api.Middleware.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Orders.Queries.GetCustomerSpendingQuery;

public class GetCustomerSpendingHandler : IRequestHandler<GetCustomerSpendingQuery, decimal>
{
    private readonly AppDbContext _context;
    
    public GetCustomerSpendingHandler(AppDbContext context, ILogger<GetCustomerSpendingHandler> logger)
    {
        _context = context;
    }

    public async Task<decimal> Handle(GetCustomerSpendingQuery request, CancellationToken cancellationToken)
    {
        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == request.CustomerId, cancellationToken);

        if (!customerExists)
        {
            throw new NotFoundException($"Customer with ID {request.CustomerId} not found");
        }
        
        return await _context.Orders
            .Where(o => o.CustomerId == request.CustomerId && !o.IsCancelled)
            .SumAsync(o => o.Price, cancellationToken);
    }
}