using AllPhi.Api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Customers.Queries.GetCustomerSpendingQuery;

public class GetCustomerSpendingHandler : IRequestHandler<GetCustomerSpendingQuery, decimal>
{
    private readonly AppDbContext _context;

    public GetCustomerSpendingHandler(AppDbContext context) => _context = context;

    public async Task<decimal> Handle(GetCustomerSpendingQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == request.CustomerId && !o.IsCancelled)
            .SumAsync(o => o.Price, cancellationToken);
    }
}