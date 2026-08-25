
using System;
using System.Xml.Linq;
using ChapinWarriors.Models.Entidades;
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Services
{
    public class CargadorConfiguracion
    {
        public void CargarDesdeXml(string rutaArchivo, AlmacenDatos almacen)
        {
            var doc = XDocument.Load(rutaArchivo);
            var raiz = doc.Root ?? throw new Exception("El archivo XML está vacío o mal formado.");

            var listaCiudades = raiz.Element("listaCiudades");
            if(listaCiudades != null)
            {
                foreach(var nodoCiudad in listaCiudades.Elements("ciudad"))
                {
                    var ciudad = ProcesarCiudad(nodoCiudad);
                    almacen.RegistrarCiudad(ciudad);
                }
            }

            var robots = raiz.Element("robots");
            if(robots != null)
            {
                foreach(var nodoRobot in robots.Elements("robot"))
                {
                    var robot = ProcesarRobot(nodoRobot);
                    almacen.RegistrarRobot(robot);
                }
            }
        }

        private Ciudad ProcesarCiudad(XElement nodoCiudad)
        {
            var nombreelem = nodoCiudad.Element("nombre")
            ?? throw new Exception("Una ciudad no tiene etiqueta <nombre>.");
            int filas = int.Parse(nombreelem.Attribute("filas")!.Value);
            int columnas = int.Parse(nombreelem.Attribute("columnas")!.Value);
            string nombre = nombreelem.Value.Trim();

            var ciudad = new Ciudad { Nombre = nombre, Filas = filas, Columnas = columnas};
            
            for (int f = 0; f < filas; f++)
            {
                var filaLista = new ListaSimple();
                for (int c = 0; c < columnas; c++)
                {
                    filaLista.Agregar(new Celda {Fila = f, Columna = c, Tipo = TipoCelda.Intransitable});
                }
                ciudad.Malla.Agregar(filaLista);
            }

            foreach (var nodoFila in nodoCiudad.Elements("fila"))
            {
                int numero = int.Parse(nodoFila.Attribute("numero")!.Value);
                string contenido = LimpiarComillas(nodoFila.Value);
                var filaCeldas = (ListaSimple)ciudad.Malla.Obtener(numero);

                for(int c = 0; c < columnas && c < contenido.Length; c++)
                {
                    var celda = (Celda)filaCeldas.Obtener(c);
                    celda.Tipo = InterpretarCaracter(contenido[c]);
                }
            }

            foreach(var nodoMilitar in nodoCiudad.Elements("unidadMilitar"))
            {
                int fila = int.Parse(nodoMilitar.Attribute("fila")!.Value);
                int columna = int.Parse(nodoMilitar.Attribute("columna")!.Value);
                int capacidad = int.Parse(nodoMilitar.Value.Trim());

                var celda = ciudad.ObtenerCelda(fila, columna);
                celda.Tipo = TipoCelda.UnidadMilitar;
                celda.CapacidadCombate = capacidad;
            }

            return ciudad;
        }

        private string LimpiarComillas(string contenido)
        {
            var texto = contenido.Trim();
            if(texto.Length >= 2 && texto.StartsWith("\"") && texto.EndsWith("\""))
            {
                texto = texto.Substring(1, texto.Length -2 );
            }
            return texto;
        }

        private TipoCelda InterpretarCaracter(char c)
        {
            return c switch
            {
                '*' => TipoCelda.Intransitable,
                ' ' => TipoCelda.Camino,
                'E' => TipoCelda.PuntoEntrada,
                'C' => TipoCelda.UnidadCivil,
                'R' => TipoCelda.Recurso,
                _ => TipoCelda.Intransitable
            };
        }

        private Robot ProcesarRobot(XElement nodoRobot)
        {
            var nombreElem = nodoRobot.Element("nombre")
            ?? throw new Exception("Un robot no tiene etiqueta <nombre>.");
            string tipo = nombreElem.Attribute("tipo")!.Value;
            string nombre = nombreElem.Value.Trim();

            if(tipo == "ChapinRescue")
            {
                return new RobotRescate {Nombre = nombre};
            }

            int capcaidad = int.Parse(nombreElem.Attribute("capacidad")!.Value);
            return new RobotFighter {Nombre = nombre, CapacidadCombate = capcaidad};
        }
    }
}