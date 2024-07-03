using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassEntidades;
using ClassLogicaNeg;
using Newtonsoft.Json;


namespace WebPresentacion
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private Grafo grafo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grafo = new Grafo();
                Session["grafo"] = grafo;
            }
            else
            {
                grafo = (Grafo)Session["grafo"];
            }
        }

        protected void ButtonAddNode_Click(object sender, EventArgs e)
        {
            var datos = new PaseLista
            {
                Fecha = DateTime.Now,
                Matricula = TextBoxMatricula.Text,
                Asistencia = CheckBoxAsistencia.Checked
            };
            grafo.InsertarNodo(datos);
            Session["grafo"] = grafo;
            UpdateGraph();
            Response.Redirect(Request.RawUrl); // Recargar la página actual
        }

        protected void ButtonAddEdge_Click(object sender, EventArgs e)
        {
            var origen = new PaseLista { Matricula = TextBoxMatriculaOrigen.Text };
            var destino = new PaseLista { Matricula = TextBoxMatriculaDestino.Text };
            var peso = double.Parse(TextBoxPeso.Text);
            grafo.InsertarArco(origen, destino, peso);
            Session["grafo"] = grafo;
            UpdateGraph();
            Response.Redirect(Request.RawUrl); // Recargar la página actual
        }

        protected void ButtonVisualize_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebForm2.aspx");
        }

        protected void ButtonTopologica_Click(object sender, EventArgs e)
        {
            var ordenTopologico = grafo.BusquedaTopologica();
            MostrarResultado("Orden Topológico:", ordenTopologico);
        }

        //private void MarcarCaminoMasCorto(Grafo grafo, Nodo origen, Nodo destino)
        //{
        //    // Reiniciar los colores de todos los nodos
        //    foreach (var nodo in grafo.Nodos)
        //    {
        //        // Asignar un color base a todos los nodos
        //        nodo.Color = Color.DarkSeaGreen;

        //    }

        //    // Marcar el camino más corto desde destino hacia origen
        //    Nodo actual = destino;
        //    while (actual != null && actual != origen)
        //    {
        //        // Asignar color verde al nodo en el camino más corto
        //        actual.Color = Color.Green;
        //        actual = actual.Padre; // Seguir al nodo padre
        //    }
        //}

        protected void ButtonDijkstra_Click(object sender, EventArgs e)
        {
            string matriculaOrigen = TextBoxMatriculaOrigen.Text.Trim();
            string matriculaDestino = TextBoxMatriculaDestino.Text.Trim();

            var nodoOrigen = grafo.Nodos.Find(n => n.Datos.Matricula == matriculaOrigen);
            var nodoDestino = grafo.Nodos.Find(n => n.Datos.Matricula == matriculaDestino);

            if (nodoOrigen != null && nodoDestino != null)
            {
                var distancias = grafo.Dijkstra(nodoOrigen);
                double distancia = distancias.ContainsKey(nodoDestino) ? distancias[nodoDestino] : double.PositiveInfinity;
                MostrarResultado($"Distancia más corta desde {matriculaOrigen} a {matriculaDestino}:", new List<Nodo> { nodoDestino });
            }
            else
            {
                LabelResultado.Text = "Nodos de origen o destino no encontrados";
            }
        }
        

    protected void MostrarResultado(string titulo, List<Nodo> nodos)
    {
        // Método para mostrar el resultado en el Label
        LabelResultado.Text = $"<strong>{titulo}</strong><br/>";
        foreach (var nodo in nodos)
        {
            LabelResultado.Text += $"{nodo.Datos.Matricula} ";
        }
    }

    protected void ButtonDFS_Click(object sender, EventArgs e)
        {
            var grafo = (Grafo)Session["grafo"];
            if (grafo != null)
            {
                // Supongamos que el origen es el primer nodo en la lista
                var origen = grafo.Nodos.FirstOrDefault();

                var recorridoDFS = grafo.DFS(origen);

                // Mostrar el resultado en el Label
                MostrarResultado("Recorrido DFS:", recorridoDFS);
            }
        }

        protected void ButtonBFS_Click(object sender, EventArgs e)
        {
            var recorridoBFS = grafo.BFS(grafo.Nodos[0]); // Suponemos el primer nodo como origen
            MostrarResultado("Recorrido BFS:", recorridoBFS);
        }

        [System.Web.Services.WebMethod]
        public static string UpdateGraph()
        {
            var grafo = (Grafo)HttpContext.Current.Session["grafo"];

            var nodesData = grafo.ObtenerNodos();
            var edgesData = grafo.ObtenerArcos();

            DibujoListaWeb dibujoWeb = new DibujoListaWeb();
            byte[] imageBytes = dibujoWeb.DibujaGrafo(nodesData, new SolidBrush(Color.LightGreen), new SolidBrush(Color.LightSalmon));

            string base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }


        [System.Web.Services.WebMethod]
        public static string GetGraphData()
        {
            // Obtener el grafo desde la sesión
            var grafo = (Grafo)HttpContext.Current.Session["grafo"];

            // Obtener nodos y arcos del grafo
            var nodes = grafo.ObtenerNodos();
            var edges = grafo.ObtenerArcos();

            // Generar la imagen del grafo en formato byte[]
            DibujoListaWeb dibujador = new DibujoListaWeb();
            byte[] imageBytes = dibujador.DibujaGrafo(nodes, new SolidBrush(Color.LightGreen), new SolidBrush(Color.LightSalmon));

            // Convertir la imagen a base64
            string base64Image = Convert.ToBase64String(imageBytes);

            // Construir un objeto con los datos del grafo y la imagen como base64
            var graphData = new { nodes = nodes, edges = edges, graphImage = base64Image };

            // Convertir el objeto a JSON y devolverlo
            return JsonConvert.SerializeObject(graphData);
        }


    }
}