using AllPhi.Api.Features.Orders.Commands.CancelOrderCommand;
using AllPhi.Api.Features.Orders.Commands.CreateOrderCommand;
using AllPhi.Api.Features.Orders.Dtos;
using AllPhi.Api.Features.Orders.Queries.GetCustomerOrdersQuery;
using AllPhi.Api.Features.Orders.Queries.GetCustomerSpendingQuery;
using AllPhi.Api.Features.Orders.Queries.GetMonthlyRevenueQuery;
using AllPhi.Api.Features.Orders.Queries.SearchOrdersQuery;
using AllPhi.Api.Middleware.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AllPhi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _logger = logger;

    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        _logger.LogInformation("Received request to create order for customer {CustomerId}", orderDto.CustomerId);
        
        try
        {
            var order = await _mediator.Send(new CreateOrderCommand(orderDto));

            _logger.LogInformation("Successfully created order {OrderId}", order.Id);

            return StatusCode(StatusCodes.Status201Created, order);
        }
        catch (NotFoundException ex)
        {   
            _logger.LogWarning(ex, "Error while creating order");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while creating order");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> GetCustomerOrders(int customerId)
    {
        _logger.LogInformation("Retrieving orders for customer {CustomerId}", customerId);

        try
        {
            var orders = await _mediator.Send(new GetCustomerOrdersQuery(customerId));
            
            _logger.LogInformation("Retrieved {OrderCount} orders for customer {CustomerId}", orders.Count, customerId);
            return Ok(orders);
        }
        catch (Exception ex)
        {   
            _logger.LogWarning(ex, "Error while retrieving orders for customer {CustomerId} ", customerId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> CancelOrder(int id)
    {
        _logger.LogInformation("Attempting to cancel order {OrderId}", id);
        
        try
        {
            var order = await _mediator.Send(new CancelOrderCommand(id));
            _logger.LogInformation("Successfully cancelled order {OrderId}", id);
            return Ok(order);
        }
        catch (NotFoundException ex)
        {   
            _logger.LogWarning(ex, "Error while cancelling order {OrderId}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while cancelling order {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> SearchOrders(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int[]? customerId)
    {
        _logger.LogInformation("Retrieving orders for customers");

        try
        {
            var orders = await _mediator.Send(new SearchOrdersQuery(startDate, endDate, customerId));
            
            _logger.LogInformation("Retrieved orders for customer");

            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while retrieving Orders for customer");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("customer/{customerId}/spending")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetCustomerSpending(int customerId)
    {
        _logger.LogInformation("Retrieving total amount spent by a customer {CustomerId}",customerId);
        
        try
        {
            var spending = await _mediator.Send(new GetCustomerSpendingQuery(customerId));
            
            _logger.LogInformation("Successfully retrieved total amount spent by a customer {CustomerId}",customerId);
            
            return Ok(spending);
        }
        catch (NotFoundException ex)
        {   
            _logger.LogWarning(ex, "Error while retrieving total amount spent by a customer {CustomerId} ", customerId);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while retrieving total amount spent by a customer {CustomerId}",customerId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("revenue/{year}/{month}")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetMonthlyRevenue(int year, int month)
    {
        _logger.LogInformation("Retrieving monthly revenue");
        
        try
        {
            var revenue = await _mediator.Send(new GetMonthlyRevenueQuery(year, month));
            
            _logger.LogInformation("Successfully retrieved monthly revenue");
            
            return Ok(revenue);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Error while retrieving monthly revenue");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
        }
    }
}