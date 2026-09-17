using Microsoft.EntityFrameworkCore;

namespace RescatApp.Repositories
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public async Task<bool> ProbarConexion()
        {
            return await Database.CanConnectAsync();
        }

    }
}
