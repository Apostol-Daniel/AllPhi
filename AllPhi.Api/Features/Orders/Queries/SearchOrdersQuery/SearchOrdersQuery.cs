using AllPhi.Api.Features.Orders.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Orders.Queries.SearchOrdersQuery;

public record SearchOrdersQuery(DateTime? StartDate, DateTime? EndDate, int? CustomerId) 
    : IRequest<List<OrderDto>>;