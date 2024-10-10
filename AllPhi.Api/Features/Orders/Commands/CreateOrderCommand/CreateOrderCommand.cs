using AllPhi.Api.Features.Orders.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Orders.Commands.CreateOrderCommand;

public record CreateOrderCommand(CreateOrderDto Order) : IRequest<OrderDto>;
