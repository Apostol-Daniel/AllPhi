using AllPhi.Api.Features.Orders.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Orders.Queries.GetCustomerOrdersQuery;

public record GetCustomerOrdersQuery(int CustomerId) : IRequest<List<OrderDto>>;
