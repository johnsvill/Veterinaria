using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Veterinaria.Models;

namespace Veterinaria.Repositorio
{
    public class Facturas : IFacturas
    {
        private readonly string _connectionDb;

        public Facturas()
        {

        }

        public Facturas(IConfiguration configuration)
        {
            this._connectionDb = configuration
                .GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<FacturaDetalle>> ListaFacturaDetalle()
        {
            using var connection = new SqlConnection(this._connectionDb);

            return await connection.QueryAsync<FacturaDetalle>("EXEC sp_ObtenerDetalleFacturas");   
        }

        public async Task<FacturaPdfModel> ObtenerFacturaPorIdAsync(int idFactura)
        {
            using var connection = new SqlConnection(_connectionDb);

            var detalles = await connection.QueryAsync<DetalleFacturaPdf>(
                "sp_ObtenerFacturaPorId",
                new { IdFactura = idFactura },
                commandType: CommandType.StoredProcedure
            );

            var encabezado = detalles.FirstOrDefault();
            if (encabezado == null) return null;

            return new FacturaPdfModel
            {
                IdFactura = idFactura,
                NombreCompleto = encabezado.NombreCompleto,
                Direccion = encabezado.Direccion,
                Telefono = encabezado.Telefono,
                Detalles = detalles.ToList()
            };
        }
    }
}
