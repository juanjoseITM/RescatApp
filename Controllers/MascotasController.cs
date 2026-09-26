using Microsoft.AspNetCore.Mvc;
using RescatApp.Identities;
using RescatApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET: Listar con filtros
        [HttpGet]
        public async Task<ActionResult<List<Mascota>>> Get([FromQuery] string? especie, [FromQuery] string? tamano)
        {
            var mascotas = await _mascotasService.ObtenerMascotasAsync(especie, tamano);
            return Ok(mascotas);
        }

        // GET: Consultar por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Mascota>> GetById(int id)
        {
            var mascota = await _mascotasService.ObtenerPorIdAsync(id);
            if (mascota == null) return NotFound(new { mensaje = "Mascota no encontrada." });
            return Ok(mascota);
        }

        // POST:Crear/Publicar mascota
        [HttpPost]
        public async Task<ActionResult<Mascota>> Post([FromBody] Mascota mascota)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevaMascota = await _mascotasService.CrearMascotaAsync(mascota);
            return CreatedAtAction(nameof(GetById), new { id = nuevaMascota.id_mascota }, nuevaMascota);
        }

        // PUT: Actualizar
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Mascota mascota)
        {
            var actualizado = await _mascotasService.ActualizarMascotaAsync(id, mascota);
            if (!actualizado) return NotFound(new { mensaje = "Mascota no encontrada para actualizar." });

            return NoContent();
        }

        // DELETE: Eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _mascotasService.EliminarMascotaAsync(id);
            if (!eliminado) return NotFound(new { mensaje = "Mascota no encontrada para eliminar." });

            return Ok(new { mensaje = "Mascota eliminada correctamente." });
        }
    }
}
