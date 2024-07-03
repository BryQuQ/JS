using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using ClassEntidades;
using System.Drawing.Imaging;
using System.IO;

namespace WebPresentacion
{
    public class DibujoListaWeb
    {
        private Bitmap imagen1;
        private Graphics papel;
        private Dictionary<Nodo, Point> posiciones;

        public DibujoListaWeb()
        {
            imagen1 = new Bitmap(912, 1100); // Tamaño del bitmap
            papel = Graphics.FromImage(imagen1);
            posiciones = new Dictionary<Nodo, Point>();
        }

        public byte[] GenerarImagenBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imagen1.Save(ms, ImageFormat.Png);
                return ms.ToArray(); // Devuelve los bytes de la imagen
            }
        }

        public byte[] DibujaGrafo(List<Nodo> nodos, SolidBrush b1, SolidBrush b2)
        {
            // Crear un nuevo bitmap y un objeto Graphics basado en ese bitmap
            Bitmap imagenGrafo = new Bitmap(912, 1100);
            Graphics papel = Graphics.FromImage(imagenGrafo);

            int width = imagenGrafo.Width; // Ancho del Bitmap
            int height = imagenGrafo.Height; // Alto del Bitmap

            // Obtener o inicializar las posiciones desde la sesión
            Dictionary<Nodo, Point> posiciones;
            if (HttpContext.Current.Session["posiciones"] == null)
            {
                posiciones = new Dictionary<Nodo, Point>();
            }
            else
            {
                posiciones = (Dictionary<Nodo, Point>)HttpContext.Current.Session["posiciones"];
            }

            Random rnd = new Random();

            // Dibuja los nodos y arcos
            foreach (var nodo in nodos)
            {
                // Si el nodo ya tiene una posición almacenada, la usara y sino genera una nueva posición aleatoria
                Point posicion;
                if (posiciones.ContainsKey(nodo))
                {
                    posicion = posiciones[nodo];
                }
                else
                {
                    // Generar posiciones aleatorias dentro del área visible
                    int posX = rnd.Next(50, width - 100); // 
                    int posY = rnd.Next(50, height - 100); // 
                    posicion = new Point(posX, posY);
                    posiciones.Add(nodo, posicion); // Almacenar la nueva posición para el nodo
                }

                // Dibuja el nodo como un círculo
                papel.FillEllipse(new SolidBrush(Color.LightGreen), posicion.X - 15, posicion.Y - 15, 30, 30); // Ejemplo con círculo
                papel.DrawEllipse(new Pen(Color.Black), posicion.X - 15, posicion.Y - 15, 30, 30);

                // Dibuja el texto del nodo 
                papel.DrawString(nodo.Datos.ToString(), new Font("Arial", 10), Brushes.Black, posicion.X - 15, posicion.Y - 8);

                // Actualizar la posición almacenada en la sesión
                posiciones[nodo] = posicion;

                // Dibuja los arcos del nodo actual
                foreach (var arco in nodo.Arcos)
                {
                    var destino = arco.Destino;
                    Point posicionDestino = posiciones[destino]; // Obtener la posición del nodo destino

                    // Dibujar una línea entre el nodo actual y su destino
                    papel.DrawLine(new Pen(Color.Black), posicion.X, posicion.Y, posicionDestino.X, posicionDestino.Y);

                    // Dibujar una flecha en el extremo del arco
                    DibujaFlecha(new Pen(Color.Black), papel, posicion.X, posicion.Y, posicionDestino.X, posicionDestino.Y);
                }
            }

            // Almacenar las posiciones actualizadas en la sesión
            HttpContext.Current.Session["posiciones"] = posiciones;

            // Convertir el bitmap a bytes
            MemoryStream ms = new MemoryStream();
            imagenGrafo.Save(ms, ImageFormat.Png);
            byte[] imageBytes = ms.ToArray();
            ms.Close();

            // Devolver los bytes de la imagen generada
            return imageBytes;
        }

        private void DibujaFlecha(Pen pen, Graphics papel, int x1, int y1, int x2, int y2)
        {
            // Calcula los puntos para dibujar la flecha
            double angle = Math.Atan2(y2 - y1, x2 - x1);
            double offset = 10; // Tamaño de la flecha

            // Puntos de la flecha
            Point arrowPoint1 = new Point((int)(x2 - offset * Math.Cos(angle - Math.PI / 6)), (int)(y2 - offset * Math.Sin(angle - Math.PI / 6)));
            Point arrowPoint2 = new Point((int)(x2 - offset * Math.Cos(angle + Math.PI / 6)), (int)(y2 - offset * Math.Sin(angle + Math.PI / 6)));

            // Dibuja la línea y la flecha
            papel.DrawLine(pen, x1, y1, x2, y2);
            papel.DrawLine(pen, x2, y2, arrowPoint1.X, arrowPoint1.Y);
            papel.DrawLine(pen, x2, y2, arrowPoint2.X, arrowPoint2.Y);
        }

    }
}