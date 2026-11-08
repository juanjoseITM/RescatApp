namespace RescatApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Clave { get; set; }
        public TipoUsuario Rol { get; set; } = TipoUsuario.Adoptante;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}