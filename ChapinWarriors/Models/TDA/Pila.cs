
// TDA DEL TIPO PILA -------------------------------------------------------------------------

namespace ChapinWarriors.Models.TDA
{
    public class Pila<T>
    {
        private Nodo<T>? tope;
        private int longitud;

        public int Longitud => longitud;
        public bool EstaVacia => longitud == 0;

        public void Apilar(T dato)
        {
            var nuevo = new Nodo<T>(dato) {Siguiente = tope};
            tope = nuevo;
            longitud++;
        }

        public T Desapilar()
        {
            if(tope == null) throw new InvalidOperationException("La pila esta vacia.");
            var dato = tope.Dato;
            tope = tope.Siguiente;
            longitud--;
            return dato;
        }

        public T VerTope()
        {
            if(tope == null) throw new InvalidOperationException("La pila está vacia.");
            return tope.Dato;
        }
    }
}