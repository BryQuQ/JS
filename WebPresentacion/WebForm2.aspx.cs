using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLogicaNeg;
using ClassEntidades;
using Newtonsoft.Json; //vis para graficar

namespace WebPresentacion
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        private Grafo grafo;

        protected void Page_Load(object sender, EventArgs e)
        {
            grafo = (Grafo)Session["grafo"];
            if (grafo != null)
            {
                var nodes = grafo.Nodos.Select(n => new
                {
                    id = n.Datos.Matricula,
                    label = n.Datos.Matricula,
                    color = n.Datos.Asistencia ? "lightgreen" : "lightsalmon"
                }).ToList();

                var edges = new List<object>();
                foreach (var nodo in grafo.Nodos)
                {
                    foreach (var arco in nodo.Arcos)
                    {
                        edges.Add(new
                        {
                            from = nodo.Datos.Matricula,
                            to = arco.Destino.Datos.Matricula,
                            label = arco.Peso.ToString()
                        });
                    }
                }

                var graphData = new { nodes = nodes, edges = edges };

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(graphData));
                Response.End();
            
        }
        }
    }
}
