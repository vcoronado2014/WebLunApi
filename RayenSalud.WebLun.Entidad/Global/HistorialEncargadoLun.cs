using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Global
{
    public class HistorialEncargadoLun : EntidadBase
    {
        #region propiedades
        private DateTime _fechaRegistro;
        public DateTime FechaRegistro
        {
            get { return _fechaRegistro; }
            set { _fechaRegistro = value; }
        }
        private string _userEncargado;
        public string UserEncargado
        {
            get { return _userEncargado; }
            set { _userEncargado = value; }
        }
        private string _runEncargado;
        public string RunEncargado
        {
            get { return _runEncargado; }
            set { _runEncargado = value; }
        }
        private string _usuarioCreador;
        public string UsuarioCreador
        {
            get { return _usuarioCreador; }
            set { _usuarioCreador = value; }
        }
        private int _tipoMovimiento;
        public int TipoMovimiento
        {
            get { return _tipoMovimiento; }
            set { _tipoMovimiento = value; }
        }

        #endregion
    }
}
