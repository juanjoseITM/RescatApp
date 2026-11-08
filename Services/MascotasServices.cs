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
            if (mascota.fecha_ingreso == null)
            {
                mascota.fecha_ingreso = DateTime.Now;
            }

            // Estado por defecto al publicar (1 = Disponible, confirmar con el equipo)
            if (mascota.id_estado_adopcion == 0)
            {
                mascota.id_estado_adopcion = 1;
            }

            int idGenerado = await _mascotasRepository.CrearAsync(mascota);
            mascota.id_mascota = idGenerado;
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
