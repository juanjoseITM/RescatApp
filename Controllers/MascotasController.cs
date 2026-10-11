using Microsoft.AspNetCore.Mvc;
using RescatApp.Models;
using RescatApp.Services;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotasController : ControllerBase
    {
        private readonly MascotasServices _mascotasService;

        public MascotasController(MascotasServices mascotasService)
        {
            _mascotasService = mascotasService;
        }

        /// <summary>
        /// Obtiene el listado de mascotas registradas, con opción de filtrar por especie o tamaño.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas([FromQuery] string? especie, [FromQuery] string? tamano)
        {
            try
            {
                var mascotas = await _mascotasService.ObtenerMascotasAsync(especie, tamano);
                return Ok(mascotas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error interno al consultar las mascotas.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la información detallada de una mascota por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var mascota = await _mascotasService.ObtenerPorIdAsync(id);
                if (mascota == null)
                {
                    return NotFound(new { mensaje = $"No se encontró la mascota con ID {id}." });
                }
                return Ok(mascota);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al consultar la mascota.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Registra/Publica una nueva mascota en el sistema.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Mascota mascota)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevaMascota = await _mascotasService.CrearAsync(mascota);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaMascota.IdMascota }, nuevaMascota);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al guardar la mascota.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza los datos de una mascota existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Mascota mascota)
        {
            if (id != mascota.IdMascota)
            {
                return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID de la mascota." });
            }

            try
            {
                bool actualizado = await _mascotasService.ActualizarAsync(mascota);
                if (!actualizado)
                {
                    return NotFound(new { mensaje = $"No se pudo actualizar. La mascota con ID {id} no existe." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la mascota.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Elimina el registro de una mascota.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                bool eliminado = await _mascotasService.EliminarAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = $"No se pudo eliminar. La mascota con ID {id} no existe." });
                }
                return Ok(new { mensaje = $"Mascota con ID {id} eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar la mascota.", detalle = ex.Message });
            }
        }
    }
}
