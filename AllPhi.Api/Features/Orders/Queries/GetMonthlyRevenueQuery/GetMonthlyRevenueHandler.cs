using AllPhi.Api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Orders.Queries.GetMonthlyRevenueQuery;

public class GetMonthlyRevenueHandler : IRequestHandler<GetMonthlyRevenueQuery, decimal>
{
    private readonly AppDbContext _context;

    public GetMonthlyRevenueHandler(AppDbContext context) => _context = context;

    public async Task<decimal> Handle(GetMonthlyRevenueQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.CreationDate.Year == request.Year && 
                        o.CreationDate.Month == request.Month && 
                        !o.IsCancelled)
            .SumAsync(o => o.Price, cancellationToken);
    }
}