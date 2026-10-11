using RescatApp.Interfaces;
using RescatApp.Models;

namespace RescatApp.Services
{
    public class MascotasServices
    {
        private readonly IMascotasRepository _mascotasRepository;

        public MascotasServices(IMascotasRepository mascotasRepository)
        {
            _mascotasRepository = mascotasRepository;
        }

        public async Task<List<Mascota>> ObtenerMascotasAsync(string? especie, string? tamano)
        {
            return await _mascotasRepository.ObtenerMascotasAsync(especie, tamano);
        }

        public async Task<Mascota?> ObtenerPorIdAsync(int id)
        {
            return await _mascotasRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Mascota> CrearAsync(Mascota mascota)
        {
            // Si no se especifica fecha de ingreso, asigna la fecha actual
            if (mascota.FechaIngreso == default)
            {
                mascota.FechaIngreso = DateTime.Now;
            }

            // Estado por defecto al publicar
            if (string.IsNullOrWhiteSpace(mascota.EstadoAdopcion))
            {
                mascota.EstadoAdopcion = "Disponible";
            }

            int idGenerado = await _mascotasRepository.CrearAsync(mascota);
            mascota.IdMascota = idGenerado;
            return mascota;
        }

        public async Task<bool> ActualizarAsync(Mascota mascota)
        {
            return await _mascotasRepository.ActualizarAsync(mascota);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _mascotasRepository.EliminarAsync(id);
        }
    }
}
