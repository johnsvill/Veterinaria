using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public interface IFacturas
    {
        Task<IEnumerable<FacturaDetalle>> ListaFacturaDetalle();
        Task<FacturaPdfModel> ObtenerFacturaPorIdAsync(int idFactura);
    }
}
