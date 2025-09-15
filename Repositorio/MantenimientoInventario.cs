using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public class MantenimientoInventario : IMantenimientoInventario
    {
        private readonly string _connectionDb;

        public MantenimientoInventario()
        {

        }

        public MantenimientoInventario(IConfiguration configuration)
        {
            this._connectionDb = configuration
                .GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ListaProductosInventario>> ListaMedicamentos()
        {
            using var connection = new SqlConnection(this._connectionDb);

            return await connection.QueryAsync<ListaProductosInventario>("EXEC ListaMedicamentos"); 
        }

        public async Task RegistrarCompraAsync(RegistrarCompraProducto compra)
        {
            using (var connection = new SqlConnection(this._connectionDb))
            {
                await connection.OpenAsync();

                var parametros = new DynamicParameters();
                parametros.Add("@IdMascota", compra.IdMascota, DbType.Int32);
                parametros.Add("@IdMedicamento", compra.IdMedicamento, DbType.Int32);
                parametros.Add("@Cantidad", compra.Cantidad, DbType.Int32);

                await connection.ExecuteAsync(
                    "sp_RegistrarCompraMedicamento",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
