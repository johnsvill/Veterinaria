namespace Veterinaria.Models
{
    public class ListaMascotas
    {
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNac { get; set; }
        public decimal PesoLibras { get; set; }
        public string NombrePersona { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string RutaFoto { get; set; }
        public string Foto { get; set; }
    }
}
