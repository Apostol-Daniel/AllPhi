using AllPhi.Api.Features.Orders.Dtos;
using MediatR;

namespace AllPhi.Api.Features.Orders.Commands.CancelOrderCommand;

public record CancelOrderCommand(int OrderId) : IRequest<OrderDto>;
