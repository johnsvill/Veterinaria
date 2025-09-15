using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public interface IMantenimientoInventario
    {
        Task<IEnumerable<ListaProductosInventario>> ListaMedicamentos();
        Task RegistrarCompraAsync(RegistrarCompraProducto compra);
    }
}
