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
        private readonly IUsuariosServices _usuariosService;
        private readonly JwtService _jwtService;

        public UsuariosController(JwtService jwtService, IUsuariosServices usuariosService)
        {
            _jwtService = jwtService;
            _usuariosService = usuariosService;
        }

        /// <summary>
        /// Obtiene la lista de todos los usuarios registrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _usuariosService.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _usuariosService.ObtenerPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });

            return Ok(usuario);
        }

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        [HttpGet("{email}")]
        public async Task<IActionResult> ObtenerUsuarioPorEmail(string email)
        {
            var usuario = await _usuariosService.ObtenerPorEmailAsync(email);
            if (usuario == null)
                return NotFound(new { mensaje = $"Usuario con Correo {email} no encontrado." });

            return Ok(usuario);
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
               
            if (await _usuariosService.EmailEnUsoAsync(usuario.Email))
            {
                return Conflict(new { mensaje = $"El correo {usuario.Email} ya está en uso por otro usuario." });
            }

            int nuevoId = await _usuariosService.CrearAsync(usuario);
            usuario.IdUsuario = nuevoId;
            usuario.Clave = null; // No devolvemos la clave en la respuesta por seguridad

            return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = nuevoId }, usuario);
        }

        /// <summary>
        /// Autentica a un usuario y genera un token JWT.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuariosService.ValidarCredencialesAsync(request.Email, request.Clave);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas. Verifica tu correo y contraseña." });

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

        /// <summary>
        ///  Actualiza la información de un usuario existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuario)
        {
            var existencia = await _usuariosService.ObtenerPorIdAsync(id);

            if (existencia == null)
            {
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
            }

            if (await _usuariosService.EmailEnUsoAsync(usuario.Email, id))
            {
                return Conflict(new { mensaje = $"El correo {usuario.Email} ya está en uso por otro usuario." });
            }

            usuario.IdUsuario = id; // Aseguramos que el ID del usuario a actualizar sea el correcto

            var actualizado = await _usuariosService.ActualizarAsync(usuario);

            if (!actualizado)
            {
                return StatusCode(500, new { mensaje = "Error interno al actualizar el usuario." });
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un usuario existente por su ID.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var existencia = await _usuariosService.ObtenerPorIdAsync(id);

            if (existencia == null)
            {
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
            }

            var eliminado = await _usuariosService.EliminarAsync(id);

            if (!eliminado)
            {
                return StatusCode(500, new { mensaje = "Error interno al eliminar el usuario." });
            }

            return Ok(new { mensaje = $"Usuario con ID {id} eliminado correctamente." });
        }

    }
}