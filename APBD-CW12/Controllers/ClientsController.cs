using APBD_CW12.Data;
using APBD_CW12.Exceptions;
using APBD_CW12.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw12.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClientsController:ControllerBase
{
    private readonly IClientsService _service;

    public ClientsController(IClientsService service)
    {
        _service = service;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.Delete(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ClientHasTripsException e)
        {
            return Conflict(e.Message);
        }
    }
}