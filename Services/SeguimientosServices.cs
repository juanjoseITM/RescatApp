using RescatApp.Identities;
using RescatApp.Repositories;

namespace RescatApp.Services
{
    public class SeguimientosServices
    {
        private readonly SeguimientosRepository _repository;

        public SeguimientosServices(SeguimientosRepository repository)
        {
            _repository = repository;
        }

        public List<Seguimiento> ObtenerTodos()
        {
            return _repository.ObtenerTodos();
        }

        public bool Guardar(Seguimiento seguimiento)
        {
            return _repository.Guardar(seguimiento);
        }
    }
}