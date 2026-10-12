using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PruebaController : ControllerBase
    {
        private readonly string _connectionString;

        public PruebaController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
        }

        [HttpGet("conexion")]
        public async Task<IActionResult> ProbarConexion()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return Ok(new { mensaje = "Conexión a la base de datos exitosa mediante SQL Client." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al conectar con la base de datos.", detalle = ex.Message });
            }
        }
    }
}