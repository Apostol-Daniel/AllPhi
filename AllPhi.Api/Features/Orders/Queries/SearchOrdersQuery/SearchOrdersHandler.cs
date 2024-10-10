using AllPhi.Api.Data;
using AllPhi.Api.Features.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Orders.Queries.SearchOrdersQuery;

public class SearchOrdersHandler : IRequestHandler<SearchOrdersQuery, List<OrderDto>>
{
    private readonly AppDbContext _context;

    public SearchOrdersHandler(AppDbContext context) => _context = context;

    public async Task<List<OrderDto>> Handle(SearchOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders.AsQueryable();

        if (request.StartDate.HasValue)
            query = query.Where(o => o.CreationDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(o => o.CreationDate <= request.EndDate.Value);

        if (request.CustomerId.HasValue)
            query = query.Where(o => o.CustomerId == request.CustomerId.Value);

        return await query
            .Select(o => new OrderDto(o.Id, o.Description, o.Price, 
                o.CreationDate, o.IsCancelled, o.CustomerId))
            .ToListAsync(cancellationToken);
    }
}