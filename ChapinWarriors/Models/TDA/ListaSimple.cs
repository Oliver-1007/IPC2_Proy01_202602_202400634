
using System;

namespace ChapinWarriors.Models.TDA
{
    public delegate bool CriterioBusqueda(object dato);

    public class ListaSimple
    {
        private Nodo? Cabeza;
        private Nodo? Cola;
        private int longitud;

        public int Longitud => longitud;
        public bool EstaVacia => longitud == 0;

        public object this[int indice]
        {
            get => Obtener(indice);
            set => Actualizar(indice, value);
        }

        public void Agregar(object dato)
        {
            var nuevo = new Nodo(dato);
            if(Cabeza == null)
            {
                Cabeza = nuevo;
                Cola = nuevo;
            }
            else
            {
                Cola!.Siguiente = nuevo;
                Cola = nuevo;
            }
            longitud++;
        }

        public object Obtener(int indice)
        {
            var nodo = ObtenerNodo(indice);
            return nodo.Dato;
        }

        public void Actualizar(int indice, object valor)
        {
            var nodo = ObtenerNodo(indice);
            nodo.Dato = valor;
        }

        private Nodo ObtenerNodo(int indice)
        {
            if(indice < 0 || indice >= longitud)
            {
                throw new IndexOutOfRangeException("Indice fuera de rango en la ListaSimple");
            }
            var actual = Cabeza;
            for(int i = 0; i < indice; i++)
            {
                actual = actual!.Siguiente;
            }
            return actual!;
        }

        public object? Buscar(CriterioBusqueda criterio)
        {
            var actual = Cabeza;
            while (actual != null)
            {
                if(criterio(actual.Dato))
                {
                    return actual.Dato;
                }
                actual = actual.Siguiente;
            }
            return default;
        }


    }
}