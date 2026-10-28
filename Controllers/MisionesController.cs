using DSW_3P_EJ01.Models;
using DSW_3P_EJ01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_EJ01.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MisionesController : ControllerBase
{
    private readonly IMisionService _service;

    public MisionesController(IMisionService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Mision>), StatusCodes.Status200OK)]
    public ActionResult<List<Mision>> ObtenerTodas()
    {
        return Ok(_service.ObtenerTodas());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Mision> ObtenerPorId(int id)
    {
        try
        {
            return Ok(_service.ObtenerPorId(id));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Mision> Agregar([FromBody] Mision nueva)
    {
        try
        {
            var creada = _service.Agregar(nueva);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(int id, [FromBody] Mision actualizada)
    {
        try
        {
            _service.Actualizar(id, actualizada);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        try
        {
            _service.Eliminar(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}
