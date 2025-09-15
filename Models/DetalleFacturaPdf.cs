namespace Veterinaria.Models
{
    public class DetalleFacturaPdf
    {
        public int IdFactura { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Medicamento { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
