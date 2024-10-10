using AllPhi.Api.Data;
using AllPhi.Api.Features.Customers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllPhi.Api.Features.Customers.Queries;

public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly AppDbContext _context;

    public GetCustomersHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName, c.Email))
            .ToListAsync(cancellationToken);
    }
}