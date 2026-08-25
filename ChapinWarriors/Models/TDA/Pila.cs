
using System;

// TDA DEL TIPO PILA -------------------------------------------------------------------------

namespace ChapinWarriors.Models.TDA
{
    public class Pila
    {
        private Nodo? tope;
        private int longitud;

        public int Longitud => longitud;
        public bool EstaVacia => longitud == 0;

        public void Apilar(object dato)
        {
            var nuevo = new Nodo(dato) {Siguiente = tope};
            tope = nuevo;
            longitud++;
        }

        public object Desapilar()
        {
            if(tope == null) throw new InvalidOperationException("La pila esta vacia.");
            var dato = tope.Dato;
            tope = tope.Siguiente;
            longitud--;
            return dato;
        }

        public object VerTope()
        {
            if(tope == null) throw new InvalidOperationException("La pila está vacia.");
            return tope.Dato;
        }
    }
}