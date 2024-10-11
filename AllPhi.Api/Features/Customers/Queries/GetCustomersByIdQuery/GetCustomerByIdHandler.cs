using AllPhi.Api.Data;
using AllPhi.Api.Features.Customers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Customers.Queries.GetCustomersByIdQuery;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly AppDbContext _context;

    public GetCustomerByIdHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .Where(c => c.Id == request.Id)
            .Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName, c.Email))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
    }
}