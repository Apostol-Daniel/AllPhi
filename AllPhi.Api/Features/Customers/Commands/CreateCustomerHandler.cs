using AllPhi.Api.Data;
using AllPhi.Api.Data.Models;
using AllPhi.Api.Features.Customers.Dtos;
using AllPhi.Api.Middleware.Exceptions;
using MediatR;

namespace AllPhi.Api.Features.Customers.Commands;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly AppDbContext _context;

    public CreateCustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            FirstName = request.customer.FirstName,
            LastName = request.customer.LastName,
            Email = request.customer.Email
        };

        bool isEmailAlreadyInUse = _context.Customers.Any(c => c.Email == request.customer.Email);

        if (isEmailAlreadyInUse)
        {
            throw new DuplicateEmailException("Email already in use");
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return new CustomerDto(customer.Id, customer.FirstName, customer.LastName, customer.Email);
    }
}