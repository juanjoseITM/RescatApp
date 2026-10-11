using RescatApp.Models;
using RescatApp.Repositories;

namespace RescatApp.Services
{
    public class SeguimientosServices
    {
        private readonly SeguimientosRepository _seguimientosRepository;

        public SeguimientosServices(SeguimientosRepository seguimientosRepository)
        {
            _seguimientosRepository = seguimientosRepository;
        }

        public async Task<List<SeguimientoPostadopcion>> ObtenerPorMascotaIdAsync(int idMascota)
        {
            return await _seguimientosRepository.ObtenerPorMascotaIdAsync(idMascota);
        }

        public async Task<int> CrearAsync(SeguimientoPostadopcion seguimiento)
        {
            if (seguimiento.FechaSeguimiento == default)
            {
                seguimiento.FechaSeguimiento = DateTime.Now;
            }

            return await _seguimientosRepository.CrearAsync(seguimiento);
        }
    }
}