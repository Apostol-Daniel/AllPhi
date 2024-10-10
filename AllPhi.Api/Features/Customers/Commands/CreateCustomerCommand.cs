using AllPhi.Api.Features.Customers.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Customers.Commands;

public record CreateCustomerCommand(CreateCustomerDto customer) : IRequest<CustomerDto>;