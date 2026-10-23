using RescatApp.Models;
using RescatApp.Repositories;

namespace RescatApp.Services
{
    public class NotificacionesServices
    {
        private readonly NotificacionesRepository _repository;

        public NotificacionesServices(NotificacionesRepository repository)
        {
            _repository = repository;
        }

        public List<Notificacion> ObtenerPorUsuario(int idUsuario)
        {
            return _repository.ObtenerPorUsuario(idUsuario);
        }

        public bool Crear(Notificacion notificacion)
        {
            return _repository.Crear(notificacion);
        }
    }
}