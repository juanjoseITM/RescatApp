namespace RescatApp.Models
{
    public class Foto_Mascota
    {

        public int id_foto { get; set; }
        public string? utl_foto { get; set; }
        public int orden { get; set; }
        public int id_mascota { get; set; }
        public Mascota? Mascota { get; set; }

    }
}
