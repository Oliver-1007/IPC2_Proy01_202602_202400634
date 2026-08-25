

using ChapinWarriors.Models.Entidades;
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Services
{
    public class AlmacenDatos
    {
        public ListaSimple Ciudades {get;} = new();
        public ListaSimple Robots {get;} = new();

        public void RegistrarCiudad(Ciudad ciudad)
        {
            for(int i = 0; i < Ciudades.Longitud; i++)
            {
                if(((Ciudad)Ciudades.Obtener(i)).Nombre == ciudad.Nombre)
                {
                    Ciudades.Actualizar(i, ciudad);
                    return;
                }
            }
            Ciudades.Agregar(ciudad);
        }

        public void RegistrarRobot(Robot robot)
        {
            for(int i = 0; i < Robots.Longitud; i++)
            {
                if(((Robot)Robots.Obtener(i)).Nombre == robot.Nombre)
                {
                    Robots.Actualizar(i, robot);
                    return;
                }
            }
            Robots.Agregar(robot);
        }

        public Ciudad? BuscarCiudad(string nombre) => (Ciudad?)Ciudades.Buscar(c => ((Ciudad)c).Nombre == nombre);
        public Robot? BuscarRobot(string nombre) => (Robot?)Robots.Buscar(r => ((Robot)r).Nombre == nombre);
    }
}