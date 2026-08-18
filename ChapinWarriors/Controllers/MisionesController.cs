using Microsoft.AspNetCore.Mvc;
using ChapinWarriors.Services;
using ChapinWarriors.Models.Entidades;

namespace ChapinWarriors.Controllers
{
    public class MisionesController : Controller
    {
        private readonly AlmacenDatos _almacen;
        private readonly MotorMisiones _motor;
        private readonly GeneradorGraphviz _graphviz;

        public MisionesController(AlmacenDatos almacen, MotorMisiones motor, GeneradorGraphviz graphviz)
        {
            _almacen = almacen;
            _motor = motor;
            _graphviz = graphviz;
        }

        [HttpGet]
        public IActionResult Seleccionar() => View(_almacen);

        /// <summary>
        /// Devuelve en JSON las celdas objetivo disponibles (unidades civiles o recursos)
        /// segun la ciudad y el tipo de mision elegidos. Se usa desde JavaScript en la vista.
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerDetalleCiudad(string nombreCiudad, string tipoMision)
        {
            var ciudad = _almacen.BuscarCiudad(nombreCiudad);
            if (ciudad == null) return NotFound();

            var tipoCelda = tipoMision == "rescate" ? TipoCelda.UnidadCivil : TipoCelda.Recurso;
            var celdas = ciudad.ObtenerCeldasPorTipo(tipoCelda);

            var lista = new List<object>();
            for (int i = 0; i < celdas.Longitud; i++)
            {
                var celda = celdas[i];
                lista.Add(new { fila = celda.Fila, columna = celda.Columna });
            }

            return Json(lista);
        }

        [HttpPost]
        public IActionResult Ejecutar(string nombreCiudad, string tipoMision, string nombreRobot, int filaObjetivo, int columnaObjetivo)
        {
            var ciudad = _almacen.BuscarCiudad(nombreCiudad);
            var robot = _almacen.BuscarRobot(nombreRobot);

            if (ciudad == null || robot == null)
            {
                TempData["Error"] = "Ciudad o robot no encontrado. Verifique la configuracion cargada.";
                return RedirectToAction("Seleccionar");
            }

            var objetivo = ciudad.ObtenerCelda(filaObjetivo, columnaObjetivo);

            ResultadoMision resultado;
            if (tipoMision == "rescate" && robot is RobotRescate robotRescate)
            {
                resultado = _motor.EjecutarRescate(ciudad, robotRescate, objetivo);
            }
            else if (tipoMision == "extraccion" && robot is RobotFighter robotFighter)
            {
                resultado = _motor.EjecutarExtraccion(ciudad, robotFighter, objetivo);
            }
            else
            {
                resultado = new ResultadoMision
                {
                    Exitosa = false,
                    TipoMision = tipoMision,
                    NombreCiudad = ciudad.Nombre,
                    Mensaje = "El robot seleccionado no es compatible con el tipo de mision."
                };
            }

            var dot = _graphviz.GenerarDot(ciudad, resultado);
            ViewBag.Dot = dot;
            ViewBag.Svg = _graphviz.GenerarSvg(dot);
            ViewBag.Ciudad = ciudad;
            return View("Resultado", resultado);
        }
    }
}