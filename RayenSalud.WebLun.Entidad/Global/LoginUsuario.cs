using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Global
{
    public class LoginUsuario
    {
        public int Id { get; set; }
        public DateTime FechaHoraLogin { get; set; }
        public string NombreUsuario { get; set; }
        public int EcolId { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
        public int FechaEntera { get; set; }

    }
}
