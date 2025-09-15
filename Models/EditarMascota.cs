namespace Veterinaria.Models
{
    public class EditarMascota
    {
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNac { get; set; }
        public decimal PesoLibras { get; set; }
        public string FotoMascota { get; set; }

        public string Ruta { get; set; }

        public IFormFile Foto { get; set; }
    }
}
