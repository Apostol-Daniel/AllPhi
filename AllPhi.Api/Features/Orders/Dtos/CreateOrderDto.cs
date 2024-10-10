namespace AllPhi.Api.Features.Orders.Dtos;

public record CreateOrderDto(
    string Description,
    decimal Price,
    int CustomerId);