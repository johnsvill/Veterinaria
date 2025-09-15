using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public class MantenimientoClientes : IMantenimientoClientes
    {
        private readonly string _connectionDb;

        public MantenimientoClientes()
        {
                
        }

        public MantenimientoClientes(IConfiguration configuration)
        {
            this._connectionDb = configuration
                .GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ListaPersonas>> ListaClientes()
        {
            using var connection = new SqlConnection(this._connectionDb);

            return await connection.QueryAsync<ListaPersonas>("EXEC ListaPersonas");
        }

        public async Task ActualizarCliente(ActualizarListaPersonas cliente)
        {
            using var connection = new SqlConnection(_connectionDb);

            await connection.ExecuteAsync(
                "ActualizarPersonaCliente",
                new
                {
                    cliente.IdPersona,
                    cliente.Nombre,
                    cliente.Direccion,
                    cliente.Telefono,
                    cliente.Email,
                    cliente.DPI,
                    cliente.NIT,
                    cliente.FechaNac
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task EliminarCliente(int IdPersona)
        {
            using var connection = new SqlConnection(_connectionDb);

            var query = @"
                    UPDATE Pj1_Cliente SET Estado = 0 WHERE IdPersona = @IdPersona;
                    UPDATE Pj1_Persona SET Estado = 0 WHERE IdPersona = @IdPersona;";

            await connection.ExecuteAsync(query, new { IdPersona });
        }

        public async Task CrearPersonaCliente(CrearPersonaCliente persona)
        {
            using var connection = new SqlConnection(_connectionDb);

            await connection.ExecuteAsync(
                "CrearPersonaCliente",
                new
                {
                    NombreCompleto = persona.Nombre,
                    persona.Telefono,
                    persona.Email,
                    persona.DPI,
                    persona.NIT,
                    persona.FechaNac,
                    persona.Genero,
                    persona.Departamento,
                    persona.Municipio,
                    persona.Zona,
                    Descripcion = persona.Direccion,                   
                },
                commandType: CommandType.StoredProcedure
            );
        }


    }
}
