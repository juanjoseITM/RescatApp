using RescatApp.Identities;

namespace RescatApp.Repositories
{
    public class UsuariosRepository
    {

        private readonly AppDbContext _context;

        public UsuariosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync() { 
        
            return await _context.Usuarios.ToListAsync();

        }

    }
}
