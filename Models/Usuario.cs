using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Usuario
    {

        [Key]
        public int id_usuario { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? correo { get; set; }
        public string? contrasena { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }
        public int tipo_usuario { get; set; }
        public DateTime? fecha_registro { get; set; }
        public int? id_refugio { get; set; }
        public Refugio? Refugio
        {
            get; set;

        }
    }
}
