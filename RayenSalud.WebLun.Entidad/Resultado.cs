using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad
{
    public class Resultado
    {
        public object Datos { get; set; }
        public Mensaje Mensaje { get; set; }
    }
    public class Mensaje
    {
        public int Codigo { get; set; }
        public string Texto { get; set; }
    }
}
