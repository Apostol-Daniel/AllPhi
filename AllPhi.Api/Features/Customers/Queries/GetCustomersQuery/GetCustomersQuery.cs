using AllPhi.Api.Features.Customers.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Customers.Queries;

public record GetCustomersQuery : IRequest<List<CustomerDto>>;