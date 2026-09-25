using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Refugio
    {
        [Key]
        public int id_refugio { get; set; }
        public string? nombre_refugio { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public string? descripcion { get; set; }
    }
}
