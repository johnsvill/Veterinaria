namespace Veterinaria.Models
{
    public class ActualizarListaPersonas
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }
        public string DPI { get; set; }

        public string NIT { get; set; }

        public DateTime FechaNac { get; set; }
    }
}
