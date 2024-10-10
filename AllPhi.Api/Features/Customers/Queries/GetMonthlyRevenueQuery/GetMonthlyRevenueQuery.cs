using MediatR;

namespace AllPhi.Api.Features.Customers.Queries.GetMonthlyRevenueQuery;

public record GetMonthlyRevenueQuery(int Year, int Month) : IRequest<decimal>;
