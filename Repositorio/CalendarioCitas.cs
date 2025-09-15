using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public class CalendarioCitas : ICalendarioCitas
    {
        private readonly string _connectionDb;

        public CalendarioCitas()
        {

        }

        public CalendarioCitas(IConfiguration configuration)
        {
            this._connectionDb = configuration
                .GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ListaCitasMascotas>> 
            ListaCalendarioCitasMascotas(DateTime fechaInicio, DateTime fechaFin)
        {
            using var connection = new SqlConnection(this._connectionDb);

            return await connection.QueryAsync<ListaCitasMascotas>(
                "CitasMascotas",
                new { FechaInicio = fechaInicio, FechaFin = fechaFin },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
