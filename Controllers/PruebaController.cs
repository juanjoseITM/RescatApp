using Microsoft.AspNetCore.Mvc;
using RescatApp.Repositories;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PruebaController : ControllerBase
    {   
        private readonly AppDbContext _context;

        public PruebaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ProbarConexion()
        {
            bool conectado = await _context.ProbarConexion();

            if (conectado)
                return Ok("Conexión exitosa con la base de datos.");

            return BadRequest("No se pudo conectar con la base de datos.");
        }
    }
}
