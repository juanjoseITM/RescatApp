using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class SolicitudAdopcion
    {
        [Key]
        public int IdSolicitud { get; set; }
        public int IdMascota { get; set; }
        public int IdAdoptante { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public string EstadoSolicitud { get; set; } = "Pendiente";
        public string? Observaciones { get; set; }
    }
}