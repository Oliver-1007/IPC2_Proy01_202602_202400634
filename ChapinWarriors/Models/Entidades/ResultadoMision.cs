
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Models.Entidades
{
    public class ResultadoMision
    {
        public bool Exitosa {get; set;}
        public string TipoMision {get; set;} = "";
        public string NombreCiudad {get; set;} = "";
        public string RobotUtilizado {get; set;} = "";
        public int CapacidadInicial {get; set;}
        public int CapacidadFinal {get; set;}
        public ListaSimple<Celda> Ruta {get; set;} = new();
        public Celda? Objetivo {get; set;}
        public String Mensaje {get; set;} = "";
    }
}