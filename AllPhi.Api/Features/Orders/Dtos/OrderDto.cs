namespace AllPhi.Api.Features.Orders.Dtos;

public record OrderDto(
    int Id,
    string Description,
    decimal Price,
    DateTime CreationDate,
    bool IsCancelled,
    int CustomerId);