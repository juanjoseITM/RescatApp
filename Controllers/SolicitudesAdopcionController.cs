using Microsoft.AspNetCore.Mvc;
using RescatApp.Interfaces;
using RescatApp.Models;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesAdopcionController : ControllerBase
    {
        private readonly ISolicitudesAdopcionRepository _repository;

        public SolicitudesAdopcionController(ISolicitudesAdopcionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene el listado completo de solicitudes de adopción
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            try
            {
                var solicitudes = await _repository.ObtenerTodasAsync();
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al obtener las solicitudes.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Consulta una solicitud por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var solicitud = await _repository.ObtenerPorIdAsync(id);
                if (solicitud == null)
                {
                    return NotFound(new { mensaje = $"No se encontró la solicitud de adopción con ID {id}." });
                }
                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al consultar la solicitud.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Consulta las solicitudes de adopción realizadas por un usuario
        /// </summary>
        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> ObtenerPorUsuario(int idUsuario)
        {
            try
            {
                var solicitudes = await _repository.ObtenerPorUsuarioAsync(idUsuario);
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al consultar las solicitudes del usuario.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Registra una nueva solicitud
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] SolicitudAdopcion solicitud)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int nuevoId = await _repository.CrearAsync(solicitud);
                solicitud.IdSolicitud = nuevoId;
                return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoId }, solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al registrar la solicitud.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza el estado de una solicitud (Aprobada, Rechazada, Pendiente)
        /// </summary>
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                return BadRequest(new { mensaje = "El nuevo estado no puede estar vacío." });
            }

            try
            {
                bool actualizado = await _repository.ActualizarEstadoAsync(id, nuevoEstado);
                if (!actualizado)
                {
                    return NotFound(new { mensaje = $"No se pudo actualizar. La solicitud con ID {id} no existe." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el estado de la solicitud.", detalle = ex.Message });
            }
        }
    }
}