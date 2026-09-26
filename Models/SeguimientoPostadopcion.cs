using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class SeguimientoPostadopcion
    {
        [Key]
        public int id_seguimiento { get; set; }
        public DateTime? fecha_seguimiento { get; set; }
        public decimal? peso_actual { get; set; }
        public string? estado_salud { get; set; }
        public string? observaciones { get; set; }
        public string? foto_evidencia { get; set; }
        public int id_mascota { get; set; }
        public int id_adoptante { get; set; }
    }
}