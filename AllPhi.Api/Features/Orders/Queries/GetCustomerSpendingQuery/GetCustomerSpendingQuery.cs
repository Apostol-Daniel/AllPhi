using MediatR;

namespace AllPhi.Api.Features.Orders.Queries.GetCustomerSpendingQuery;

public record GetCustomerSpendingQuery(int CustomerId) : IRequest<decimal>;
