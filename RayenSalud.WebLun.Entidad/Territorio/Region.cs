using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Territorio
{
    public class Region : EntidadBase
    {
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        private string _codigoDeis;
        public string CodigoDeis
        {
            get { return _codigoDeis; }
            set { _codigoDeis = value; }
        }
    }
}
