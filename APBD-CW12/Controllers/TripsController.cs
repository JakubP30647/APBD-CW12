using cw12.Exceptions;
using cw12.Models.DTOs;
using cw12.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw12.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return Ok(await _tripsService.GetTrips(pageNumber, pageSize));
    }

    [HttpPost("{tripId}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute] int tripId, [FromBody] ClientToTripDTO clientToTripDTO)
    {
        try
        {
            await _tripsService.AddClientToTrip(tripId, clientToTripDTO);
            return Created(nameof(AddClientToTrip), clientToTripDTO);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadDateException e)
        {
            return BadRequest(e.Message);
        }
        catch (ClientOnTripException e)
        {
            return Conflict(e.Message);
        }
    }
}