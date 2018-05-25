using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Parametro
{
    public class Nodo
    {
        #region Constructor
        public Nodo()
        {
        }
        #endregion

        #region propiedades publicas
        public System.Int32 Id { get; set; }
        public System.Int32 IdRegion { get; set; }
        public System.Int32 IdComuna { get; set; }
        public System.Int32 IdNodo { get; set; }
        public System.String RazonSocial { get; set; }
        public System.Int32 Tipo { get; set; }
        #endregion
    }
}
