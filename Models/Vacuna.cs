using System.ComponentModel.DataAnnotations;

namespace RescatApp.Models
{
    public class Vacuna
    {

        [Key]
        public int id_vacuna { get; set; }
        public string? nombre_vacuna { get; set; }

    }
}
