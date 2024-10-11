using AllPhi.Api.Features.Customers.Commands;
using AllPhi.Api.Features.Customers.Dtos;
using AllPhi.Api.Features.Customers.Queries.GetCustomerByEmailQuery;
using AllPhi.Api.Features.Customers.Queries.GetCustomersByIdQuery;
using AllPhi.Api.Features.Customers.Queries.GetCustomersQuery;
using AllPhi.Api.Middleware.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public CustomersController(IMediator mediator, ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _logger =logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
    {
        _logger.LogInformation("Retrieving customers");
        
        try
        {
            var customers = await _mediator.Send(new GetCustomersQuery());
            
            _logger.LogInformation("Successfully retrieved customers");
            
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while retrieving customers");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        _logger.LogInformation("Retrieving customer {CustomerId}", id);
        try
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
            
            if (customer is null)
            {
                _logger.LogWarning("Customer {CustomerId} not found", id);
                return NotFound();
            }
            
            _logger.LogInformation("Successfully retrieved {CustomerId}", id);

            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while retrieving {CustomerId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
        
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerDto>>> SearchCustomers([FromQuery] string email)
    {
        _logger.LogInformation("Searching customers by email {Email}", email);

        try
        {
            var customers = await _mediator.Send(new GetCustomerByEmailQuery(email));
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while retrieving by email {Email}", email);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerDto customerDto)
    {
        _logger.LogInformation("Creating new customer");

        try
        {
            var customer = await _mediator.Send(new CreateCustomerCommand(customerDto));

            _logger.LogInformation("New customer successfully created");

            return StatusCode(StatusCodes.Status201Created, customer);
        }
        catch (DuplicateEmailException dex)
        {
            _logger.LogInformation(dex,"Duplicate email found when creating new customer");
            return BadRequest("Email already in use. Please choose another one");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while creating customer");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
    }
}