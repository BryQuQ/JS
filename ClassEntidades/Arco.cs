using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassEntidades
{
    public class Arco
    {
        public Nodo Destino { get; set; }
        public double Peso { get; set; }

        public Arco(Nodo destino, double peso)
        {
            Destino = destino;
            Peso = peso;
        }
    }
}
