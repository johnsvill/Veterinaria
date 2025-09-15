namespace Veterinaria.Models
{
    public class ListaCitasMascotas
    {
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Veterinario { get; set; }
        public string Especialidad { get; set; }
        public string Observaciones { get; set; }
        public string FechaHoraConsulta { get; set; }        
    }
}
