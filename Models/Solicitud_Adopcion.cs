using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Solicitud_Adopcion
    {

        [Key]
        public int id_solicitud { get; set; }
        public DateTime? fecha_solicitud { get; set; }
        public string? motivo { get; set; }
        public int id_estado_solicitud { get; set; }
        public DateTime? fecha_respuesta { get; set; }
        public string? observaciones_rescatista { get; set; }
        public int id_mascota { get; set; }
        public Mascota? Mascota { get; set; }
        public int id_usuario { get; set; }
        public Usuario? Usuario { get; set; }


    }
}
