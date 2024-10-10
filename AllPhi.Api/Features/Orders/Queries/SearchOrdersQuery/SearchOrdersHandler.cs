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

        if (request.CustomersId is not null && request.CustomersId.Any())
            query = query.Where(o => request.CustomersId.Contains(o.CustomerId));

        return await query
            .Select(o => new OrderDto(o.Id, o.Description, o.Price, 
                o.CreationDate, o.IsCancelled, o.CustomerId))
            .ToListAsync(cancellationToken);
    }
}