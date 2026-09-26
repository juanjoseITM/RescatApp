using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Mascota
    {
        [Key]
        public int id_mascota { get; set; }
        public string? nombre { get; set; }
        public string? especie { get; set; }
        public string? raza { get; set; }
        public int? edad_aprox { get; set; }
        public string? sexo { get; set; }
        public int id_tamaño { get; set; }
        public int id_estado_salud { get; set; }
        public string? descripcion { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public int id_estado_adopcion { get; set; }
    }
}
