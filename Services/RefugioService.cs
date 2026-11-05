using System.Collections.Generic;
using RescatApp.Models;
using RescatApp.Repositories;

namespace RescatApp.Services
{
    public class RefugioService
    {
        private readonly RefugioRepository _refugioRepository;

        public RefugioService(RefugioRepository refugioRepository)
        {
            _refugioRepository = refugioRepository;
        }

        public List<Refugio> ListarRefugios()
        {
            return _refugioRepository.ObtenerTodos();
        }

        public void CrearRefugio(Refugio refugio)
        {
            _refugioRepository.Insertar(refugio);
        }
    }
}