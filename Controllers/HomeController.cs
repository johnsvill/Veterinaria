using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        public HomeController(ILogger<HomeController> logger, IMantenimientoClientes mantenimientoClientes,
            IMantenimientoMascotas mantenimientoMascotas)
        {
            this._logger = logger;
            this._mantenimientoClientes = mantenimientoClientes;
            this._mantenimientoMascotas = mantenimientoMascotas;
        }

        public IActionResult Index()
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
    }
}
