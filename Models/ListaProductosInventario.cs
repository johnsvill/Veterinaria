namespace Veterinaria.Models
{
    public class ListaProductosInventario
    {
        public int IdMedicamento { get; set; }
        public string Medicamento { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
