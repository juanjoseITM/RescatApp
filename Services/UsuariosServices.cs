using RescatApp.Interfaces;
using RescatApp.Models;
namespace RescatApp.Services
{
    public class UsuariosServices
    {
        
        // El repositorio que el servicio va a usar
        private readonly IUsuariosRepository _usuariosRepository;

        // Program.cs entrega el repositorio aca (inyeccion de dependencias)
        public UsuariosServices(IUsuariosRepository usuariosRepository)
        {
            
            _usuariosRepository = usuariosRepository;

        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            
            return await _usuariosRepository.ObtenerTodosAsync();

        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            
            return await _usuariosRepository.ObtenerPorIdAsync(id);

        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            
            return await _usuariosRepository.ObtenerPorEmailAsync(email);

        }

        public async Task<int> CrearAsync(Usuario usuario)
        {
            
            return await _usuariosRepository.CrearAsync(usuario);

        }

    }
}
