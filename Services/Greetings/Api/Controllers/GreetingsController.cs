using Microsoft.AspNetCore.Mvc;
using Onion.Assignment.Services.Greetings.Api.DTOs;
using Onion.Assignment.Services.Greetings.Domain.Interfaces;

namespace Onion.Assignment.Services.Greetings.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GreetingsController : ControllerBase
{
    private readonly IGreetingUseCases _greetingUseCases;

    public GreetingsController(IGreetingUseCases greetingUseCases)
    {
        _greetingUseCases = greetingUseCases ?? throw new ArgumentNullException(nameof(greetingUseCases));
    }

    /// <summary>
    /// Creates a new greeting for the specified name
    /// </summary>
    /// <param name="request">The greeting creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created greeting</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GreetingResponse), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<ActionResult<GreetingResponse>> CreateGreeting(
        [FromBody] CreateGreetingRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var greeting = await _greetingUseCases.CreateGreetingAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetGreetingById), new { id = greeting.Id }, greeting);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Creates a greeting using the legacy /hello endpoint format
    /// </summary>
    /// <param name="name">The name to greet</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The greeting message</returns>
    [HttpGet("/hello")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<ActionResult<string>> Hello(
        [FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new CreateGreetingRequest(name);
            var greeting = await _greetingUseCases.CreateGreetingAsync(request, cancellationToken);
            return Ok(greeting.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets a greeting by its ID
    /// </summary>
    /// <param name="id">The greeting ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The greeting if found</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GreetingResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GreetingResponse>> GetGreetingById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var greeting = await _greetingUseCases.GetGreetingByIdAsync(id, cancellationToken);
        
        return greeting != null ? Ok(greeting) : NotFound();
    }

    /// <summary>
    /// Gets all greetings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>All greetings</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GreetingResponse>), 200)]
    public async Task<ActionResult<IEnumerable<GreetingResponse>>> GetAllGreetings(
        CancellationToken cancellationToken = default)
    {
        var greetings = await _greetingUseCases.GetAllGreetingsAsync(cancellationToken);
        return Ok(greetings);
    }

    /// <summary>
    /// Gets greetings by name
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Greetings matching the name</returns>
    [HttpGet("by-name/{name}")]
    [ProducesResponseType(typeof(IEnumerable<GreetingResponse>), 200)]
    public async Task<ActionResult<IEnumerable<GreetingResponse>>> GetGreetingsByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var greetings = await _greetingUseCases.GetGreetingsByNameAsync(name, cancellationToken);
        return Ok(greetings);
    }
}