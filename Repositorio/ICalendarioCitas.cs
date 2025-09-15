using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public interface ICalendarioCitas
    {
        Task<IEnumerable<ListaCitasMascotas>> ListaCalendarioCitasMascotas(DateTime fechaInicio, DateTime fechaFin);
    }
}
