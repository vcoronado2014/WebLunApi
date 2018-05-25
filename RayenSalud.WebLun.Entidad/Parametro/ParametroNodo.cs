using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Parametro
{
    public class ParametroNodo
    {
        #region Constructor
        public ParametroNodo()
        {

        }
        #endregion

        #region Propiedades publicas
        public System.Int32 Id { get; set; }
        public System.String NombreEstablecimiento { get; set; }
        public System.Int32 IdNodo { get; set; }
        public System.Boolean TieneUrgencia { get; set; }
        public System.Boolean TieneAps { get; set; }
        public System.Boolean TieneHospital { get; set; }
        public System.Boolean TieneRedesAsistenciales { get; set; }
        public System.Int32 EsComuna { get; set; }
        public System.Int32 IdComuna { get; set; }
        public System.String NombreUsuario { get; set; }
        public System.Boolean EsSeleccionado { get; set; }
        #endregion
    }
}
