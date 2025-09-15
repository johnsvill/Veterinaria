using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public class MantenimientoMascotas : IMantenimientoMascotas
    {
        private readonly string _connectionDb;

        public MantenimientoMascotas()
        {

        }

        public MantenimientoMascotas(IConfiguration configuration)
        {
            this._connectionDb = configuration
                .GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ListaMascotas>> ListaMascotas()
        {
            using var connection = new SqlConnection(this._connectionDb);

            return await connection.QueryAsync<ListaMascotas>("EXEC ListaMascotas");
        }

        public async Task EditarMascota(EditarMascota mascota)
        {
            using var connection = new SqlConnection(_connectionDb);

            await connection.ExecuteAsync(
                "ActualizarMascota",
                new
                {
                    mascota.IdMascota,
                    mascota.NombreMascota,
                    mascota.Especie,
                    mascota.Raza,
                    mascota.FechaNac,
                    mascota.PesoLibras,
                    Foto = mascota.FotoMascota, 
                    RutaFoto = mascota.Ruta                              
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task EliminarMascota(int idMascota)
        {
            using var connection = new SqlConnection(_connectionDb);

            await connection.ExecuteAsync(
                "EliminarMascota",
                new { IdMascota = idMascota },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task CrearMascota(CrearMascota mascota)
        {
            using var connection = new SqlConnection(_connectionDb);

            await connection.ExecuteAsync(
                "CrearMascota",
                new
                {
                    mascota.IdCliente,
                    mascota.Nombre,
                    mascota.Especie,
                    mascota.Raza,
                    mascota.FechaNac,
                    mascota.PesoLibras,
                    Foto = mascota.FotoNombre,
                    RutaFoto = mascota.RutaFoto,
                    Activo = true
                },
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
