using AllPhi.Api.Data;
using AllPhi.Api.Features.Customers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Customers.Queries.GetCustomerByEmailQuery;

public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, List<CustomerDto>>
{
    private readonly AppDbContext _context;

    public GetCustomerByEmailQueryHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<CustomerDto>> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .Where(c => c.Email.Contains(request.email))
            .Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName, c.Email))
            .ToListAsync(cancellationToken);
    }
}