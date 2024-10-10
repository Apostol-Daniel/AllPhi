namespace AllPhi.Api.Features.Customers.Dtos;

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string Email);