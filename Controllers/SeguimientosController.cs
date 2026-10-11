using Microsoft.AspNetCore.Mvc;
using RescatApp.Models;
using RescatApp.Services;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguimientosController : ControllerBase
    {
        private readonly SeguimientosServices _seguimientosService;

        public SeguimientosController(SeguimientosServices seguimientosService)
        {
            _seguimientosService = seguimientosService;
        }

        /// <summary>
        /// Obtiene el historial de reportes de seguimiento de una mascota por su ID
        /// </summary>
        [HttpGet("mascota/{idMascota}")]
        public async Task<IActionResult> ObtenerPorMascota(int idMascota)
        {
            try
            {
                var lista = await _seguimientosService.ObtenerPorMascotaIdAsync(idMascota);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al consultar los seguimientos de la mascota.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Registra un nuevo reporte post-adopción para una mascota
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] SeguimientoPostadopcion seguimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int nuevoId = await _seguimientosService.CrearAsync(seguimiento);
                seguimiento.IdSeguimiento = nuevoId;
                return CreatedAtAction(nameof(ObtenerPorMascota), new { idMascota = seguimiento.IdMascota }, seguimiento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al registrar el seguimiento post-adopción.", detalle = ex.Message });
            }
        }
    }
}