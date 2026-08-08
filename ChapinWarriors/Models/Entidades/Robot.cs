
// CLASE ABSTRACTA DE ROBOT Y SUS HIJOS -----------------------------------------------

namespace ChapinWarriors.Models.Entidades
{
    public enum TipoRobot
    {
        ChapinRecause,
        ChapinFighter
    }
    public abstract class Robot
    {
        public string Nombre {get; set;}
        public abstract TipoRobot Tipo {get;}
    }

    public class RobotRescate : Robot
    {
        public override TipoRobot Tipo => TipoRobot.ChapinRecause;
    }

    public class RobotFighter : Robot
    {
        public int CapacidadCombate {get; set;}
        public override TipoRobot Tipo => TipoRobot.ChapinFighter;
    }
}