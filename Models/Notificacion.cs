using RescatApp.Identities;
using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Notificacion
    {

        [Key]
        public int id_notificacion { get; set; }
        public string? tipo_notificacion { get; set; }
        public string? mensaje { get; set; }
        public DateTime fecha_envio { get; set; }
        public bool leido { get; set; }
        public int id_usuario { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
