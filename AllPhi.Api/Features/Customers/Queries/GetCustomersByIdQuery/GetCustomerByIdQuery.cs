using AllPhi.Api.Features.Customers.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Customers.Queries.GetCustomersByIdQuery;

public record GetCustomerByIdQuery(int Id) : IRequest<CustomerDto>;
