using RescatApp.Models;

namespace RescatApp.Interfaces
{
    public interface ISolicitudesAdopcionRepository
    {
        Task<List<SolicitudAdopcion>> ObtenerTodasAsync();
        Task<SolicitudAdopcion?> ObtenerPorIdAsync(int id);
        Task<List<SolicitudAdopcion>> ObtenerPorUsuarioAsync(int idUsuario);
        Task<int> CrearAsync(SolicitudAdopcion solicitud);
        Task<bool> ActualizarEstadoAsync(int idSolicitud, string nuevoEstado);
    }
}