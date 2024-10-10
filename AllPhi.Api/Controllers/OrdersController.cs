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

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        var order = await _mediator.Send(new CreateOrderCommand(orderDto));
        return Created();
    }

    // [HttpGet("{id}")]
    // [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<ActionResult<OrderDto>> GetOrder(int id)
    // {
    //     var order = await _mediator.Send(new GetOrderByIdQuery(id));
    //     if (order == null)
    //         return NotFound();
    //     return Ok(order);
    // }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> GetCustomerOrders(int customerId)
    {
        var orders = await _mediator.Send(new GetCustomerOrdersQuery(customerId));
        return Ok(orders);
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> CancelOrder(int id)
    {
        try
        {
            var order = await _mediator.Send(new CancelOrderCommand(id));
            return Ok(order);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrderDto>>> SearchOrders(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? customerId)
    {
        var orders = await _mediator.Send(new SearchOrdersQuery(startDate, endDate, customerId));
        return Ok(orders);
    }

    [HttpGet("customer/{customerId}/spending")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetCustomerSpending(int customerId)
    {
        var spending = await _mediator.Send(new GetCustomerSpendingQuery(customerId));
        return Ok(spending);
    }

    [HttpGet("revenue/{year}/{month}")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetMonthlyRevenue(int year, int month)
    {
        var revenue = await _mediator.Send(new GetMonthlyRevenueQuery(year, month));
        return Ok(revenue);
    }
}