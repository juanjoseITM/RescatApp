using Microsoft.EntityFrameworkCore;
using RescatApp.Models;

namespace RescatApp.Repositories
{
    public class UsuariosRepository
    {

        private readonly AppDbContext _context;

        public UsuariosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _context.Usuarios.AnyAsync(u => u.correo != null && u.correo.ToLower() == correo.ToLower());
        }

        public async Task<Usuario> CrearUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

    }
}
