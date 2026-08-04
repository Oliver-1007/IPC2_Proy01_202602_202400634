
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Models.Entidades
{
    public class Ciudad
    {
        public String Nombre {get; set;} = "";
        public int Filas {get; set;}
        public int Columnas {get; set;}

        // Implementacion de Malla bidimensional implementada
        // como lista de lisyas
        public ListaSimple<ListaSimple<Celda>> Malla {get; set;} = new();
        public Celda ObtenerCelda(int fila, int columna)
        {
            return Malla.Obtener(fila).Obtener(columna);
        }

        public ListaSimple<Celda> ObtenerCeldasPorTipo(TipoCelda tipo)
        {
            var resultado = new ListaSimple<Celda>();
            for(int f = 0; f < Filas; f++)
            {
                for (int c = 0; c < Columnas; c++)
                {
                    var Celda = ObtenerCelda(f,c);
                    if (Celda.Tipo == tipo)
                    {
                        resultado.Agregar(Celda);
                    }
                }
            }
            return resultado;
        }
    }
}