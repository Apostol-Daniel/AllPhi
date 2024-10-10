using AllPhi.Api.Features.Customers.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Customers.Queries.GetCustomerByEmailQuery;

public record GetCustomerByEmailQuery(string email) : IRequest<List<CustomerDto>>;