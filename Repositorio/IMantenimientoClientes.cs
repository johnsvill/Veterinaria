
using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public interface IMantenimientoClientes
    {
        Task ActualizarCliente(ActualizarListaPersonas cliente);
        Task CrearPersonaCliente(CrearPersonaCliente persona);
        Task EliminarCliente(int IdPersona);
        Task<IEnumerable<ListaPersonas>> ListaClientes();
    }
}
