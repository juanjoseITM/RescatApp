using RescatApp.Models;

namespace RescatApp.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<int> CrearAsync(Usuario usuario);
    }
}