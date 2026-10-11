using RescatApp.Models;

namespace RescatApp.Interfaces
{
    public interface IMascotasRepository
    {
        Task<List<Mascota>> ObtenerTodasAsync();
        Task<Mascota?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Mascota mascota);
        Task<bool> ActualizarAsync(Mascota mascota);
        Task<bool> EliminarAsync(int id);
    }
}
