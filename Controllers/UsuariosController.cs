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

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
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

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        [HttpGet("{email}")]
        public async Task<IActionResult> ObtenerUsuarioPorEmail(string email)
        {
            try
            {
                var usuario = await _usuariosService.ObtenerPorEmailAsync(email);
                if (usuario == null)
                    return NotFound(new { mensaje = $"Usuario con Correo {email} no encontrado." });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al consultar el usuario.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existente = await _usuariosService.ObtenerPorEmailAsync(usuario.Email);

                if (existente != null)
                {

                    return Conflict(new { mensaje = $"El usuario {usuario.Nombre} ya existe con email {usuario.Email}" });

                }

                int nuevoId = await _usuariosService.CrearAsync(usuario);
                usuario.IdUsuario = nuevoId;
                usuario.Clave = null; // No devolvemos la clave en la respuesta por seguridad

                return CreatedAtAction(nameof(ObtenerUsuarioPorId), new { id = nuevoId }, usuario);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al registrar el usuario.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Autentica a un usuario y genera un token JWT.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al autenticar.", detalle = ex.Message });
            }
        }

        /// <summary>
        ///  Actualiza la información de un usuario existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuario)
        {

            try
            {

                var existencia = await _usuariosService.ObtenerPorIdAsync(id);

                if (existencia == null)
                {

                    return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });

                }

                // Validamos el correo electrónico para asegurarnos de que no se duplique con otro usuario
                var otro = await _usuariosService.ObtenerPorEmailAsync(usuario.Email);

                if (otro != null && otro.IdUsuario != id)
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
            catch (Exception ex)
            {

                return StatusCode(500, new { mensaje = "Error interno al actualizar el usuario.", detalle = ex.Message });

            }

        }

        /// <summary>
        /// Elimina un usuario existente por su ID.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
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
            catch (Exception ex)
            {

                return StatusCode(500, new { mensaje = "Error interno al eliminar el usuario.", detalle = ex.Message });

            }
        }

    }
}