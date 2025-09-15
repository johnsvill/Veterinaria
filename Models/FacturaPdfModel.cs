namespace Veterinaria.Models
{
    public class FacturaPdfModel
    {
        public int IdFactura { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public List<DetalleFacturaPdf> Detalles { get; set; }
    }
}
