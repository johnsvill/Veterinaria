namespace Veterinaria.Models
{
    public class CrearPersonaCliente
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        public string Genero { get; set; }
        public string Departamento { get; set; }

            public string Municipio { get; set; }

        public string Zona { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }
        public string DPI { get; set; }

        public string NIT { get; set; }

        public DateTime FechaNac { get; set; }
    }
}
