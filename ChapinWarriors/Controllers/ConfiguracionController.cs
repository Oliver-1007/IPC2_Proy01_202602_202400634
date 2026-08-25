using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ChapinWarriors.Services;

namespace ChapinWarriors.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly AlmacenDatos _almacen;
        private readonly CargadorConfiguracion _cargador;

        public ConfiguracionController(AlmacenDatos almacen, CargadorConfiguracion cargador)
        {
            _almacen = almacen;
            _cargador = cargador;
        }

        [HttpGet]
        public IActionResult Cargar() => View(_almacen);

        [HttpPost]
        public IActionResult Cargar(IFormFile archivoXml)
        { 
            if (archivoXml == null || archivoXml.Length == 0)
            {
                ViewBag.Error = "Debe seleccionar un archivo XML valido.";
                return View(_almacen);
            }

            var rutaTemporal = Path.GetTempFileName();
            try
            {
                using (var flujo = new FileStream(rutaTemporal, FileMode.Create))
                {
                    archivoXml.CopyTo(flujo);
                }

                _cargador.CargarDesdeXml(rutaTemporal, _almacen);
                ViewBag.Mensaje = "Archivo de configuracion cargado correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al procesar el archivo: {ex.Message}";
            }
            finally
            {
                if (System.IO.File.Exists(rutaTemporal))
                    System.IO.File.Delete(rutaTemporal);
            }

            return View(_almacen);
        }
    }
}