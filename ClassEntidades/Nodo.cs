using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassEntidades
{
    public class Nodo
    {
        public PaseLista Datos { get; set; }
        public List<Arco> Arcos { get; set; }

        public Nodo(PaseLista datos)
        {
            Datos = datos;
            Arcos = new List<Arco>();

        }
    }
}
