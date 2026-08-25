
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Models.Entidades
{
    public class Ciudad
    {
        public string Nombre {get; set;} = "";
        public int Filas {get; set;}
        public int Columnas {get; set;}

        // Implementacion de Malla bidimensional implementada
        // como lista de lisyas
        public ListaSimple Malla {get; set;} = new();
        public Celda ObtenerCelda(int fila, int columna)
        {
            return (Celda)((ListaSimple)Malla.Obtener(fila)).Obtener(columna);
        }

        public ListaSimple ObtenerCeldasPorTipo(TipoCelda tipo)
        {
            var resultado = new ListaSimple();
            for(int f = 0; f < Filas; f++)
            {
                for (int c = 0; c < Columnas; c++)
                {
                    var celda = ObtenerCelda(f,c);
                    if (celda.Tipo == tipo)
                    {
                        resultado.Agregar(celda);
                    }
                }
            }
            return resultado;
        }
    }
}