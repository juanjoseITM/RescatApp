using Microsoft.AspNetCore.Mvc;
using RescatApp.Identities;
using RescatApp.Services;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguimientosController : ControllerBase
    {
        private readonly SeguimientosServices _service;

        public SeguimientosController(SeguimientosServices service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lista = _service.ObtenerTodos();
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Seguimiento seguimiento)
        {
            var resultado = _service.Guardar(seguimiento);
            if (resultado)
                return Ok(new { mensaje = "Seguimiento guardado correctamente" });

            return BadRequest(new { mensaje = "No se pudo guardar el seguimiento" });
        }
    }
}