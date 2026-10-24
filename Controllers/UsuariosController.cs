using Microsoft.AspNetCore.Mvc;
using RescatApp.Interfaces;
using RescatApp.Models;
using RescatApp.Services;

namespace RescatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly UsuariosServices _usuariosService;
        private readonly JwtService _jwtService;

        public UsuariosController(IUsuariosRepository usuariosRepository, JwtService jwtService, UsuariosServices usuariosServices)
        {
            _usuariosRepository = usuariosRepository;
            _jwtService = jwtService;
            _usuariosService = usuariosServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuariosService.ObtenerTodosAsync();
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
                var usuario = await _usuariosService.ObtenerPorIdAsync(id);
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

                // Generar el token JWT
                var token = _jwtService.GenerarToken(usuario);

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    token = token,
                    usuario = new
                    {
                        usuario.IdUsuario,
                        usuario.Nombre,
                        usuario.Email,
                        usuario.Rol
                    }
                });
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