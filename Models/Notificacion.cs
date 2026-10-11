using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }
        public int IdUsuario { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; } = DateTime.Now;
        public bool Leido { get; set; } = false;
    }
}