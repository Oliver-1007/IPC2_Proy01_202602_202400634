

using ChapinWarriors.Models.Entidades;
using ChapinWarriors.Models.TDA;

namespace ChapinWarriors.Services
{
    public class AlmacenDatos
    {
        public ListaSimple<Ciudad> Ciudades {get;} = new();
        public ListaSimple<Robot> Robots {get;} = new();

        public void RegistrarCiudad(Ciudad ciudad)
        {
            for(int i = 0; i < Ciudades.Longitud; i++)
            {
                if(Ciudades.Obtener(i).Nombre == ciudad.Nombre)
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
                Robots.Actualizar(i, robot);
                return;
            }
            Robots.Agregar(robot);
        }

        public Ciudad? BuscarCiudad(string nombre) => Ciudades.Buscar(c => c.Nombre == nombre);
        public Robot? BuscarRobot(string nombre) => Robots.Buscar(r=> r.Nombre == nombre);
    }
}