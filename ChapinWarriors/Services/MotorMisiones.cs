

using ChapinWarriors.Models.Entidades;
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Services
{
    // MotorMision se encarga de buscar el camino que debe de tomar el robot
    // para completar una mision
    public class MotorMisiones
    {
        private static readonly int[] DF = {-1, 1, 0, 0};
        private static readonly int[] DC = {0,0,-1,1};

        // Funcion que busca cualquier entrada disponible y verifica si
        // es posible llegar a hacer el rescate
        public ResultadoMision EjecutarRescate(Ciudad ciudad, RobotRescate robot, Celda civilObjetivo)
        {
            var resultado = new ResultadoMision
            {
                TipoMision = "rescate",
                NombreCiudad = ciudad.Nombre,
                RobotUtilizado= robot.Nombre,
                Objetivo = civilObjetivo
            };

            var puntosEntrada = ciudad.ObtenerCeldasPorTipo(TipoCelda.PuntoEntrada);
            if(puntosEntrada.EstaVacia)
            {
                resultado.Exitosa = false;
                resultado.Mensaje = "Mision imposible";
                return resultado;
            }

            for(int i = 0; i < puntosEntrada.Longitud;i++)
            {
                var entrada = puntosEntrada[i];
                var ruta = BuscarCaminoSimple(ciudad, entrada, civilObjetivo);
                if(ruta != null)
                {
                    resultado.Exitosa = true;
                    resultado.Ruta = ruta;
                    return resultado;
                }
            }

            resultado.Exitosa = false;
            resultado.Mensaje = "Mision imposible";
            return resultado;
        }

        // Funcion que busca caminos transitables sin repetir ninguna hasta
        // llegar al punto civil
        private ListaSimple<Celda>? BuscarCaminoSimple(Ciudad ciudad, Celda inicio, Celda objetivo)
        {
            bool[,] visitado = new bool[ciudad.Filas, ciudad.Columnas];
            (int f, int c)[,] previo = new (int, int)[ciudad.Filas, ciudad.Columnas];

            var cola = new ListaSimple<(int f, int c)>();
            visitado[inicio.Fila, inicio.Columna] = true;
            cola.Agregar((inicio.Fila, inicio.Columna));
            int indice = 0;

            while (indice < cola.Longitud)
            {
                var actual = cola.Obtener(indice);
                indice++;

                if (actual.f == objetivo.Fila && actual.c == objetivo.Columna)
                    return ReconstruirRuta(ciudad, previo, inicio, objetivo);

                for (int d = 0; d < 4; d++)
                {
                    int nf = actual.f + DF[d];
                    int nc = actual.c + DC[d];
                    if (nf < 0 || nf >= ciudad.Filas || nc < 0 || nc >= ciudad.Columnas) continue;
                    if (visitado[nf, nc]) continue;

                    var celdaVecina = ciudad.ObtenerCelda(nf, nc);
                    bool esObjetivo = nf == objetivo.Fila && nc == objetivo.Columna;

                    if (!esObjetivo && !EsTransitablePorRescate(celdaVecina)) continue;

                    visitado[nf, nc] = true;
                    previo[nf, nc] = (actual.f, actual.c);
                    cola.Agregar((nf, nc));
                }
            }

            return null;
        }

        // Funcion boleana que responde si es posible pasar o no
        private bool EsTransitablePorRescate(Celda celda)
        {
            return celda.Tipo == TipoCelda.Camino
                || celda.Tipo == TipoCelda.PuntoEntrada
                || celda.Tipo == TipoCelda.UnidadCivil;
        }

        // Funcion que busca llegar al objetivo siempre y cuando el camino sea
        // transitable y pueda llgar con vida
        public ResultadoMision EjecutarExtraccion(Ciudad ciudad, RobotFighter robot, Celda recursoObjetivo)
        {
            var resultado = new ResultadoMision
            {
                TipoMision = "extraccion",
                NombreCiudad = ciudad.Nombre,
                RobotUtilizado = robot.Nombre,
                CapacidadInicial = robot.CapacidadCombate,
                Objetivo = recursoObjetivo
            };

            var puntosEntrada = ciudad.ObtenerCeldasPorTipo(TipoCelda.PuntoEntrada);
            if (puntosEntrada.EstaVacia)
            {
                resultado.Exitosa = false;
                resultado.Mensaje = "Mision Imposible";
                return resultado;
            }

            ListaSimple<Celda>? mejorRuta = null;
            int mejorCapacidadFinal = -1;

            for (int i = 0; i < puntosEntrada.Longitud; i++)
            {
                var entrada = puntosEntrada[i];
                var (ruta, capacidadFinal) = BuscarCaminoConCombate(ciudad, entrada, recursoObjetivo, robot.CapacidadCombate);
                if (ruta != null && capacidadFinal > mejorCapacidadFinal)
                {
                    mejorRuta = ruta;
                    mejorCapacidadFinal = capacidadFinal;
                }
            }

            if (mejorRuta == null)
            {
                resultado.Exitosa = false;
                resultado.Mensaje = "Mision Imposible";
                return resultado;
            }

            resultado.Exitosa = true;
            resultado.Ruta = mejorRuta;
            resultado.CapacidadFinal = mejorCapacidadFinal;
            return resultado;
        }

        // Funcion que busca llegar al objetivo con la mayor capacidad de combate posible
        private (ListaSimple<Celda>? ruta, int capacidadFinal) BuscarCaminoConCombate(
            Ciudad ciudad, Celda inicio, Celda objetivo, int capacidadInicial)
        {
            int[,] mejorCapacidad = new int[ciudad.Filas, ciudad.Columnas];
            for (int f = 0; f < ciudad.Filas; f++)
                for (int c = 0; c < ciudad.Columnas; c++)
                    mejorCapacidad[f, c] = -1;

            (int f, int c)[,] previo = new (int, int)[ciudad.Filas, ciudad.Columnas];
            bool[,] enCola = new bool[ciudad.Filas, ciudad.Columnas];

            mejorCapacidad[inicio.Fila, inicio.Columna] = capacidadInicial;
            var cola = new ListaSimple<(int f, int c)>();
            cola.Agregar((inicio.Fila, inicio.Columna));
            enCola[inicio.Fila, inicio.Columna] = true;
            int indice = 0;

            while (indice < cola.Longitud)
            {
                var actual = cola.Obtener(indice);
                indice++;
                enCola[actual.f, actual.c] = false;
                int capacidadActual = mejorCapacidad[actual.f, actual.c];

                for (int d = 0; d < 4; d++)
                {
                    int nf = actual.f + DF[d];
                    int nc = actual.c + DC[d];
                    if (nf < 0 || nf >= ciudad.Filas || nc < 0 || nc >= ciudad.Columnas) continue;

                    var celdaVecina = ciudad.ObtenerCelda(nf, nc);
                    bool esObjetivo = nf == objetivo.Fila && nc == objetivo.Columna;

                    if (celdaVecina.Tipo == TipoCelda.Intransitable) continue;
                    if (celdaVecina.Tipo == TipoCelda.Recurso && !esObjetivo) continue;

                    int capacidadResultante = capacidadActual;
                    if (celdaVecina.Tipo == TipoCelda.UnidadMilitar)
                    {
                        if (capacidadActual <= celdaVecina.CapacidadCombate) continue;
                        capacidadResultante = capacidadActual - celdaVecina.CapacidadCombate;
                    }

                    if (capacidadResultante > mejorCapacidad[nf, nc])
                    {
                        mejorCapacidad[nf, nc] = capacidadResultante;
                        previo[nf, nc] = (actual.f, actual.c);

                        if (!enCola[nf, nc])
                        {
                            cola.Agregar((nf, nc));
                            enCola[nf, nc] = true;
                        }
                    }
                }
            }

            if (mejorCapacidad[objetivo.Fila, objetivo.Columna] < 0)
                return (null, 0);

            var ruta = ReconstruirRuta(ciudad, previo, inicio, objetivo);
            return (ruta, mejorCapacidad[objetivo.Fila, objetivo.Columna]);
        }

        // Funcion que toma los datos dejados en "previo" para reconstruir la ruta hallada
        private ListaSimple<Celda> ReconstruirRuta(Ciudad ciudad, (int f, int c)[,] previo, Celda inicio, Celda objetivo)
        {
            var pila = new Pila<Celda>();
            var actual = (objetivo.Fila, objetivo.Columna);

            while (!(actual.Item1 == inicio.Fila && actual.Item2 == inicio.Columna))
            {
                pila.Apilar(ciudad.ObtenerCelda(actual.Item1, actual.Item2));
                actual = previo[actual.Item1, actual.Item2];
            }
            pila.Apilar(inicio);

            var ruta = new ListaSimple<Celda>();
            while (!pila.EstaVacia)
                ruta.Agregar(pila.Desapilar());

            return ruta;
        }
    }
}
