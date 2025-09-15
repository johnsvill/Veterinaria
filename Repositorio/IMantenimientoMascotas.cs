using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public interface IMantenimientoMascotas
    {
        Task CrearMascota(CrearMascota mascota);
        Task EditarMascota(EditarMascota mascota);
        Task EliminarMascota(int idMascota);
        Task<IEnumerable<ListaMascotas>> ListaMascotas();
    }
}
