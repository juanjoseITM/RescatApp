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

        // Métodos principales esperados por MascotasController:

        public async Task<List<Mascota>> ObtenerMascotasAsync(string? especie = null, string? tamano = null)
        {
            return await _mascotasRepository.ObtenerMascotasAsync(especie, tamano);
        }

        public async Task<Mascota?> ObtenerPorIdAsync(int id)
        {
            return await _mascotasRepository.ObtenerPorIdAsync(id);
        }

        public async Task<bool> CrearAsync(Mascota mascota)
        {
            if (string.IsNullOrWhiteSpace(mascota.EstadoAdopcion))
            {
                mascota.EstadoAdopcion = "Disponible";
            }

            var idGenerado = await _mascotasRepository.CrearAsync(mascota);
            return idGenerado > 0;
        }

        public async Task<bool> ActualizarAsync(Mascota mascota)
        {
            return await _mascotasRepository.ActualizarAsync(mascota);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _mascotasRepository.EliminarAsync(id);
        }

        // Alias compatibles por si alguna otra clase los llama con "Mascota":
        public async Task<List<Mascota>> ObtenerTodasAsync(string? especie = null, string? tamano = null)
            => await ObtenerMascotasAsync(especie, tamano);

        public async Task<bool> CrearMascotaAsync(Mascota mascota)
            => await CrearAsync(mascota);

        public async Task<bool> ActualizarMascotaAsync(Mascota mascota)
            => await ActualizarAsync(mascota);

        public async Task<bool> EliminarMascotaAsync(int id)
            => await EliminarAsync(id);
    }
}