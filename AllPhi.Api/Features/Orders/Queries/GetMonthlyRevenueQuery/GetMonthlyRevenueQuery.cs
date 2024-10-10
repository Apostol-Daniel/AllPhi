using MediatR;

namespace AllPhi.Api.Features.Orders.Queries.GetMonthlyRevenueQuery;

public record GetMonthlyRevenueQuery(int Year, int Month) : IRequest<decimal>;
