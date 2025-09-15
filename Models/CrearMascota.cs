namespace Veterinaria.Models
{
    public class CrearMascota
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime FechaNac { get; set; }
        public decimal PesoLibras { get; set; }

        public IFormFile Foto { get; set; }        
        public string FotoNombre { get; set; }     
        public string RutaFoto { get; set; }
    }
}
