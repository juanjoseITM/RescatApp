using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int IdTipoUsuario { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}