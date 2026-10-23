using Microsoft.AspNetCore.Mvc;
using RescatApp.Models;
using RescatApp.Services;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly NotificacionesServices _notificacionesService;

        public NotificacionesController(NotificacionesServices notificacionesService)
        {
            _notificacionesService = notificacionesService;
        }

        // GET: api/Notificaciones/usuario/5
        [HttpGet("usuario/{idUsuario}")]
        public IActionResult ObtenerPorUsuario(int idUsuario)
        {
            var notificaciones = _notificacionesService.ObtenerPorUsuario(idUsuario);
            return Ok(notificaciones);
        }

        // POST: api/Notificaciones
        [HttpPost]
        public IActionResult Crear([FromBody] Notificacion notificacion)
        {
            if (notificacion == null)
            {
                return BadRequest("Los datos de la notificación no son válidos.");
            }

            var resultado = _notificacionesService.Crear(notificacion);
            if (resultado)
            {
                return Ok(new { mensaje = "Notificación creada exitosamente." });
            }

            return StatusCode(500, "Error al guardar la notificación.");
        }
    }
}