using RescatApp.Models;

namespace RescatApp.Interfaces
{
    public interface IUsuariosServices
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<int> CrearAsync(Usuario usuario);
        Task<bool> ActualizarAsync(Usuario usuario);
        Task<bool> EliminarAsync(int id);
        Task<Usuario?> ValidarCredencialesAsync(string email, string clave);
    }
}