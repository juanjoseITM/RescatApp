using RescatApp.Identities;
using RescatApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RescatApp.Services
{
    public class MascotasServices
    {
        private readonly MascotasRepository _repository;

        public MascotasServices(MascotasRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Mascota>> ObtenerMascotasAsync(string? especie, string? tamano)
        {
            return await _repository.ObtenerMascotasAsync(especie, tamano);
        }

        public async Task<Mascota?> ObtenerPorIdAsync(int id)
        {
            return await _repository.ObtenerPorIdAsync(id);
        }

        public async Task<Mascota> CrearMascotaAsync(Mascota mascota)
        {
            mascota.fecha_ingreso = DateTime.Now;
            if (string.IsNullOrWhiteSpace(mascota.estado_adopcion))
            {
                mascota.estado_adopcion = "Disponible";
            }

            int idGenerado = await _repository.CrearAsync(mascota);
            mascota.id_mascota = idGenerado;
            return mascota;
        }

        public async Task<bool> ActualizarMascotaAsync(int id, Mascota mascota)
        {
            var existente = await _repository.ObtenerPorIdAsync(id);
            if (existente == null) return false;

            mascota.id_mascota = id;
            return await _repository.ActualizarAsync(mascota);
        }

        public async Task<bool> EliminarMascotaAsync(int id)
        {
            return await _repository.EliminarAsync(id);
        }
    }
}
