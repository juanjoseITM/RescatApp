using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class SeguimientoPostadopcion
    {
        [Key]
        public int IdSeguimiento { get; set; }
        public DateTime FechaSeguimiento { get; set; } = DateTime.Now;
        public decimal? PesoActual { get; set; }
        public string? EstadoSalud { get; set; }
        public string? Observaciones { get; set; }
        public string? FotoEvidencia { get; set; }
        public int IdMascota { get; set; }
        public int IdAdoptante { get; set; }
    }
}