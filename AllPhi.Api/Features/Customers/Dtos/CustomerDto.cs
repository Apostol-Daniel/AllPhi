namespace AllPhi.Api.Features.Customers.Dtos;

public record CustomerDto(
    int Id,
    string FirstName,
    string LastName,
    string Email);