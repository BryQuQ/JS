using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassEntidades;

namespace ClassLogicaNeg
{
    public class Grafo
    {
        public List<Nodo> Nodos { get; private set; }

        public Grafo()
        {
            Nodos = new List<Nodo>();
        }

        public void InsertarNodo(PaseLista datos)
        {
            Nodos.Add(new Nodo(datos));
        }

        public void InsertarArco(PaseLista origen, PaseLista destino, double peso)
        {
            var nodoOrigen = Nodos.FirstOrDefault(n => n.Datos.Matricula == origen.Matricula);
            var nodoDestino = Nodos.FirstOrDefault(n => n.Datos.Matricula == destino.Matricula);

            if (nodoOrigen != null && nodoDestino != null)
            {
                nodoOrigen.Arcos.Add(new Arco(nodoDestino, peso));
            }
        }

        public List<Nodo> ObtenerNodos()
        {
            return Nodos;
        }

        public List<Arco> ObtenerArcos()
        {
            List<Arco> arcos = new List<Arco>();
            foreach (var nodo in Nodos)
            {
                arcos.AddRange(nodo.Arcos);
            }
            return arcos;
        }

        public List<Nodo> DFS(Nodo inicio)
        {
            var visitados = new List<Nodo>();
            var stack = new Stack<Nodo>();

            stack.Push(inicio);

            while (stack.Count > 0)
            {
                var nodo = stack.Pop();

                if (!visitados.Contains(nodo))
                {
                    visitados.Add(nodo);

                    foreach (var arco in nodo.Arcos)
                    {
                        stack.Push(arco.Destino);
                    }
                }
            }

            return visitados;
        }

        public List<Nodo> BFS(Nodo inicio)
        {
            var visitados = new List<Nodo>();
            var queue = new Queue<Nodo>();

            queue.Enqueue(inicio);

            while (queue.Count > 0)
            {
                var nodo = queue.Dequeue();

                if (!visitados.Contains(nodo))
                {
                    visitados.Add(nodo);

                    foreach (var arco in nodo.Arcos)
                    {
                        queue.Enqueue(arco.Destino);
                    }
                }
            }

            return visitados;
        }

        public List<Nodo> BusquedaTopologica()
        {
            var visitados = new List<Nodo>();
            var orden = new List<Nodo>();
            var stack = new Stack<Nodo>();

            foreach (var nodo in Nodos)
            {
                if (!visitados.Contains(nodo))
                {
                    TopologicoUtil(nodo, visitados, stack);
                }
            }

            while (stack.Count > 0)
            {
                orden.Add(stack.Pop());
            }

            return orden;
        }

        public void TopologicoUtil(Nodo nodo, List<Nodo> visitados, Stack<Nodo> stack)
        {
            visitados.Add(nodo);

            foreach (var arco in nodo.Arcos)
            {
                if (!visitados.Contains(arco.Destino))
                {
                    TopologicoUtil(arco.Destino, visitados, stack);
                }
            }

            stack.Push(nodo);
        }

        public Dictionary<Nodo, double> Dijkstra(Nodo origen)
        {
            var distancias = new Dictionary<Nodo, double>();
            var cola = new Queue<Nodo>();

            foreach (var nodo in Nodos)
            {
                distancias[nodo] = double.PositiveInfinity;
            }

            distancias[origen] = 0;
            cola.Enqueue(origen);

            while (cola.Count > 0)
            {
                var actual = cola.Dequeue();

                foreach (var arco in actual.Arcos)
                {
                    var distanciaNueva = distancias[actual] + arco.Peso;
                    if (distanciaNueva < distancias[arco.Destino])
                    {
                        distancias[arco.Destino] = distanciaNueva;
                        cola.Enqueue(arco.Destino);
                    }
                }
            }

            return distancias;
        }

    }
}
