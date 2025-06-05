using cw12.Data;
using cw12.Exceptions;
using cw12.Services;
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
        catch (HasTripsException e)
        {
            return Conflict(e.Message);
        }
    }
}