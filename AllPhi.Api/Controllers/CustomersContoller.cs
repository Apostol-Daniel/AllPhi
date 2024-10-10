using AllPhi.Api.Features.Customers.Commands;
using AllPhi.Api.Features.Customers.Dtos;
using AllPhi.Api.Features.Customers.Queries.GetCustomerByEmailQuery;
using AllPhi.Api.Features.Customers.Queries.GetCustomersQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
    {
        var customers = await _mediator.Send(new GetCustomersQuery());
        return Ok(customers);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerDto>>> SearchCustomers([FromQuery] string email)
    {
        var customers = await _mediator.Send(new GetCustomerByEmailQuery(email));
        return Ok(customers);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerDto customerDto)
    {
        var customer = await _mediator.Send(new CreateCustomerCommand(customerDto));
        return Created();
    }

    
}