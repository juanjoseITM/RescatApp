using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Refugio> Refugios { get; set; }
        public DbSet<Foto_Mascota> Fotos_Mascotas { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Solicitud_Adopcion> Solicitud_Adopciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<SeguimientoPostadopcion> SeguimientoPostadopcions { get; set; }
        public DbSet<Vacuna> Vacunas { get; set; }

        public async Task<bool> ProbarConexion()
        {
            return await Database.CanConnectAsync();
        }

    }
}
