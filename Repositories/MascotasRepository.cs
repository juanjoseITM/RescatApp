using Microsoft.EntityFrameworkCore;
using RescatApp.Identities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RescatApp.Repositories
{
    public class MascotasRepository
    {
        private readonly AppDbContext _context;

        public MascotasRepository(AppDbContext context)
        {
            _context = context;
        }

        // L - List / Obtener todas las mascotas (con filtros opcionales)
        public async Task<List<Mascota>> ObtenerMascotasAsync(string? especie, string? tamano)
        {
            var query = _context.Mascotas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(especie))
            {
                query = query.Where(m => m.especie.ToLower() == especie.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(tamano))
            {
                query = query.Where(m => m.tamano.ToLower() == tamano.ToLower());
            }

            return await query.ToListAsync();
        }

        // R - Read / Obtener mascota por ID
        public async Task<Mascota?> ObtenerPorIdAsync(int id)
        {
            return await _context.Mascotas
                .FirstOrDefaultAsync(m => m.id_mascota == id);
        }

        // C - Create / Crear mascota
        public async Task<Mascota> CrearAsync(Mascota mascota)
        {
            await _context.Mascotas.AddAsync(mascota);
            await _context.SaveChangesAsync();
            return mascota;
        }

        // U - Update / Actualizar mascota
        public async Task<bool> ActualizarAsync(Mascota mascota)
        {
            _context.Mascotas.Update(mascota);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // D - Delete / Eliminar mascota
        public async Task<bool> EliminarAsync(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null) return false;

            _context.Mascotas.Remove(mascota);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
