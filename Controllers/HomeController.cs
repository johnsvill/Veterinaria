using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using Veterinaria.Models;
using Veterinaria.Repositorio;

namespace Veterinaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMantenimientoClientes _mantenimientoClientes;
        private readonly IMantenimientoMascotas _mantenimientoMascotas;
        private readonly ICalendarioCitas _calendarioCitas;
        private readonly IMantenimientoInventario _mantenimientoInventario;
        private readonly IFacturas _facturas;

        public HomeController(ILogger<HomeController> logger, IMantenimientoClientes mantenimientoClientes,
            IMantenimientoMascotas mantenimientoMascotas, ICalendarioCitas calendarioCitas,
            IMantenimientoInventario mantenimientoInventario, IFacturas facturas)
        {
            this._logger = logger;
            this._mantenimientoClientes = mantenimientoClientes;
            this._mantenimientoMascotas = mantenimientoMascotas;
            this._calendarioCitas = calendarioCitas;
            this._mantenimientoInventario = mantenimientoInventario;
            this._facturas = facturas;
        }      

        public ActionResult MenuPrincipal()
        {
            return View();
        }

        public async Task<ActionResult> ListaClientes()
        {
            var datos = await this._mantenimientoClientes.ListaClientes();

            var listaClientes = datos.Select(c => new ListaPersonas
            {
                IdPersona = c.IdPersona,
                Nombre = c.Nombre,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Email = c.Email,
                DPI = c.DPI,
                NIT = c.NIT,
                FechaNac = c.FechaNac
            }).ToList();

            return View(listaClientes);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPersona(ListaPersonas nuevaPersona)
        {
            if (!ModelState.IsValid)
                return View(nuevaPersona);

            var personaMapped = new CrearPersonaCliente
            {
                IdPersona = nuevaPersona.IdPersona,
                Nombre = nuevaPersona.Nombre,
                Direccion = nuevaPersona.Direccion,
                Genero = "M",
                Departamento = "Guatemala",
                Municipio = "Mixco",
                Zona = "1",
                Telefono = nuevaPersona.Telefono,
                Email = nuevaPersona.Email,
                DPI = nuevaPersona.DPI,
                NIT = nuevaPersona.NIT,
                FechaNac = nuevaPersona.FechaNac
            };

            await _mantenimientoClientes.CrearPersonaCliente(personaMapped);
            return RedirectToAction("ListaClientes");
        }

        [HttpPost]
        public async Task<IActionResult> EditarListaPersonas(ListaPersonas cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var clienteActualizado = new ActualizarListaPersonas
            {
                IdPersona = cliente.IdPersona,
                Nombre = cliente.Nombre,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                DPI = cliente.DPI,
                NIT = cliente.NIT,
                FechaNac = cliente.FechaNac
            };

            await this._mantenimientoClientes.ActualizarCliente(clienteActualizado);
            return RedirectToAction("ListaClientes");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int IdPersona)
        {
            await _mantenimientoClientes.EliminarCliente(IdPersona);
            return RedirectToAction("ListaClientes");
        }

        public async Task<ActionResult> ListaMascotas()
        {
            var datos = await this._mantenimientoMascotas.ListaMascotas();

            var listaMascotas = datos.Select(c => new ListaMascotas
            {
                IdMascota = c.IdMascota,
                NombreMascota = c.NombreMascota,
                Especie = c.Especie,
                Raza = c.Raza,
                FechaNac = c.FechaNac,
                PesoLibras = c.PesoLibras,
                NombrePersona = c.NombrePersona,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                RutaFoto = c.RutaFoto,
                Foto = c.Foto
            }).ToList();

            return View(listaMascotas);
        }

        [HttpPost]
        public async Task<IActionResult> EditarMascota(EditarMascota mascota)
        {
            string nombreArchivo = null;
            
            if (mascota.Foto != null && mascota.Foto.Length > 0)
            {
                string extension = Path.GetExtension(mascota.Foto.FileName);
                string baseName = Path.GetFileNameWithoutExtension(mascota.Foto.FileName);
                string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                nombreArchivo = $"{baseName}_{timestamp}{extension}";
                string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", nombreArchivo);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await mascota.Foto.CopyToAsync(stream);
                }
            }
           
            var mascotaEditada = new EditarMascota
            {
                IdMascota = mascota.IdMascota,
                NombreMascota = mascota.NombreMascota,
                Especie = mascota.Especie,
                Raza = mascota.Raza,
                FechaNac = mascota.FechaNac,
                PesoLibras = mascota.PesoLibras,
                FotoMascota = nombreArchivo,
                Ruta = "C:\\Users\\johns\\OneDrive\\Documents\\Proyectos\\Veterinaria\\wwwroot\\imagenes\\"
            };

            await _mantenimientoMascotas.EditarMascota(mascotaEditada);

            return RedirectToAction("ListaMascotas");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarMascota(int IdMascota)
        {
            await _mantenimientoMascotas.EliminarMascota(IdMascota);
            return RedirectToAction("ListaMascotas");
        }

        [HttpPost]
        public async Task<ActionResult> CrearMascota(CrearMascota mascota)
        {
            string nombreArchivo = null;
            string rutaRelativa = null;

            if (mascota.Foto != null && mascota.Foto.Length > 0)
            {
                string extension = Path.GetExtension(mascota.Foto.FileName);
                string baseName = Path.GetFileNameWithoutExtension(mascota.Foto.FileName);
                string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                nombreArchivo = $"{baseName}_{timestamp}{extension}";
                string rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await mascota.Foto.CopyToAsync(stream);
                }

                rutaRelativa = "/imagenes/" + nombreArchivo;
            }

            var mascotaNueva = new CrearMascota
            {
                IdCliente = mascota.IdCliente,
                Nombre = mascota.Nombre,
                Especie = mascota.Especie,
                Raza = mascota.Raza,
                FechaNac = mascota.FechaNac,
                PesoLibras = mascota.PesoLibras,
                FotoNombre = nombreArchivo,
                RutaFoto = rutaRelativa
            };

            await _mantenimientoMascotas.CrearMascota(mascotaNueva);

            return RedirectToAction("ListaMascotas");
        } 
        
        public ActionResult CalendarioCitas()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerEventosCalendario(DateTime start, DateTime end)
        {
            var lista = await this._calendarioCitas.ListaCalendarioCitasMascotas(start, end);

            var eventos = lista
                .Where(e => !string.IsNullOrWhiteSpace(e.FechaHoraConsulta))
                .Select(e =>
                {
                    DateTime fechaConsulta;
                    bool fechaValida = DateTime.TryParse(e.FechaHoraConsulta, out fechaConsulta);

                    if (!fechaValida)
                    {                        
                        fechaConsulta = DateTime.Today;
                    }

                    return new EventoCalendario
                    {
                        Title = $"Mascota: {e.NombreMascota} | Veterinario: {e.Veterinario}" +
                                $" | Especialidad: {e.Especialidad} | Observaciones: {e.Observaciones}" +
                                $" | Fecha: {fechaConsulta:dd-MM-yyyy HH:mm}",
                        Start = fechaConsulta.ToString("yyyy-MM-ddTHH:mm:ss"),
                        End = fechaConsulta.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                        Color = fechaConsulta < DateTime.Now ? "#dc3545" : "#28a745"
                    };
                });

            return Json(eventos);
        }

        public async Task<ActionResult> ListaMedicamentos()
        {
            var lista = await _mantenimientoInventario.ListaMedicamentos();

            var productosInventario = lista.Select(c => new ListaProductosInventario
            {
                IdMedicamento = c.IdMedicamento,
                Medicamento = c.Medicamento?.Trim(),
                Cantidad = c.Cantidad,
                FechaVencimiento = c.FechaVencimiento
            }).ToList();

            return View(productosInventario);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCompraMedicamento(RegistrarCompraProducto modelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos inválidos.");
            }

            try
            {
                await this._mantenimientoInventario.RegistrarCompraAsync(modelo);
                TempData["CompraExitosa"] = "Compra registrada correctamente.";
                return RedirectToAction("ListaMedicamentos");
            }
            catch (Exception)
            {
                TempData["ErrorCompra"] = $"Error al registrar la compra!";
                return RedirectToAction("ListaMedicamentos");
            }
        }

        public async Task<IActionResult> ListaFacturas()
        {
            var lista = await this._facturas.ListaFacturaDetalle();

            return View(lista);
        }

        public byte[] GenerarFacturaPdf(FacturaPdfModel factura)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text($"Factura #{factura.IdFactura}").FontSize(20).Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Cliente: {factura.NombreCompleto}");
                        col.Item().Text($"Dirección: {factura.Direccion}");
                        col.Item().Text($"Teléfono: {factura.Telefono}");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Medicamento").Bold();
                                header.Cell().Element(CellStyle).Text("Precio").Bold();
                                header.Cell().Element(CellStyle).Text("Cantidad").Bold();
                                header.Cell().Element(CellStyle).Text("Total").Bold();
                            });

                            foreach (var item in factura.Detalles)
                            {
                                table.Cell().Element(CellStyle).Text(item.Medicamento);
                                table.Cell().Element(CellStyle).Text($"Q{item.PrecioUnitario:F2}");
                                table.Cell().Element(CellStyle).Text(item.Cantidad.ToString());
                                table.Cell().Element(CellStyle).Text($"Q{item.Total:F2}");
                            }

                            static IContainer CellStyle(IContainer container) =>
                                container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        });

                        var totalGeneral = factura.Detalles.Sum(x => x.Total);
                        col.Item().AlignRight().Text($"Total: Q{totalGeneral:F2}").FontSize(14).Bold();
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        [HttpPost]
        public async Task<IActionResult> GenerarFacturaPdf(int IdFactura)
        {
            var factura = await this._facturas.ObtenerFacturaPorIdAsync(IdFactura);

            if (factura == null) return NotFound();

            var pdfBytes = GenerarFacturaPdf(factura);

            var timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var fileName = $"Factura_{IdFactura}_{timestamp}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
