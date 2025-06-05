using APBD_CW12.Exceptions;
using APBD_CW12.Models.DTOs;
using APBD_CW12.Services;
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
    public async Task<IActionResult> Get([FromQuery] int pageNumber, [FromQuery] int pageSize)
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
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (ClientTripException e)
        {
            return Conflict(e.Message);
        }
    }
}