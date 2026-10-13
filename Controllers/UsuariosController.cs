using Microsoft.AspNetCore.Mvc;
using RescatApp.Interfaces;
using RescatApp.Models;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public UsuariosController(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuariosRepository.ObtenerTodosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al consultar los usuarios.", detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _usuariosRepository.ObtenerPorIdAsync(id);
                if (usuario == null)
                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al consultar el usuario.", detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                int nuevoId = await _usuariosRepository.CrearAsync(usuario);
                usuario.IdUsuario = nuevoId;

                return CreatedAtAction(nameof(GetUsuarioPorId), new { id = nuevoId }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al registrar el usuario.", detalle = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _usuariosRepository.ValidarCredencialesAsync(request.Email, request.Clave);
                if (usuario == null)
                    return Unauthorized(new { mensaje = "Credenciales inválidas. Verifica tu correo y contraseña." });

                return Ok(new { mensaje = "Inicio de sesión exitoso.", usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al autenticar.", detalle = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }
}