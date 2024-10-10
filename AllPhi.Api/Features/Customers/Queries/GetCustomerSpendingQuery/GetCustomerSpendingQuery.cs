using MediatR;

namespace AllPhi.Api.Features.Customers.Queries.GetCustomerSpendingQuery;

public record GetCustomerSpendingQuery(int CustomerId) : IRequest<decimal>;
