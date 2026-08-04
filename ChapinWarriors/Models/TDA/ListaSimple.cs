


namespace ChapinWarriors.Models.TDA
{
    public class ListaSimple<T>
    {
        private Nodo<T>? Cabeza;
        private Nodo<T>? Cola;
        private int longitud;

        public int Longitud => longitud;
        public bool EstaVacia => longitud == 0;

        public T this[int indice]
        {
            get => Obtener(indice);
            set => Actualizar(indice, value);
        }

        public void Agregar(T dato)
        {
            var nuevo = new Nodo<T>(dato);
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

        public T Obtener(int indice)
        {
            var nodo = ObtenerNodo(indice);
            return nodo.Dato;
        }

        public void Actualizar(int indice, T valor)
        {
            var nodo = ObtenerNodo(indice);
            nodo.Dato = valor;
        }

        private Nodo<T> ObtenerNodo(int indice)
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

        public T? Buscar(Func<T, bool> criterio)
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