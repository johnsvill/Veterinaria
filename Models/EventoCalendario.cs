namespace Veterinaria.Models
{
    public class EventoCalendario
    {
        public string Title { get; set; }
        public string Start { get; set; } // formato ISO: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss
        public string End { get; set; }
        public string Color { get; set; }
    }
}
