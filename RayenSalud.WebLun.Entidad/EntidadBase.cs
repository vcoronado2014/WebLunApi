using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad
{
    public class EntidadBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _activo;
        public int Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }
        private int _eliminado;
        public int Eliminado
        {
            get { return _eliminado; }
            set { _eliminado = value; }
        }
    }
}
