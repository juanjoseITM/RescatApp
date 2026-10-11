using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Mascota
    {
        [Key]
        public int IdMascota { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string? Raza { get; set; }
        public int? EdadAprox { get; set; }
        public string? Sexo { get; set; }
        public string? Tamano { get; set; }
        public string? EstadoSalud { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public string EstadoAdopcion { get; set; } = "Disponible";
        public int IdRescatista { get; set; }
    }
}