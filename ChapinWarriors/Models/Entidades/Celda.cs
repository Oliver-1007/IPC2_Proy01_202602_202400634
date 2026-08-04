

namespace ChapinWarriors.Models.Entidades
{
    public class Celda
    {
        public int Fila {get; set;}
        public int Columna {get; set;}
        public TipoCelda Tipo {get; set;}

        //Solo aplica para el tipo UnidadMilitar
        public int CapacidadCombate {get; set;}
    }
}