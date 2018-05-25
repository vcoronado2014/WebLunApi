using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Global
{
    public class ContratanteLun : EntidadBase
    {
        public ContratanteLun()
        {
        }
        #region propiedades
        private string _razonSocial;
        public string RazonSocial
        {
            get { return _razonSocial; }
            set { _razonSocial = value; }
        }
        private DateTime _fechaModificacion;
        public DateTime FechaModificacion
        {
            get { return _fechaModificacion; }
            set { _fechaModificacion = value; }
        }
        private DateTime _fechaInicioContrato;
        public DateTime FechaInicioContrato
        {
            get { return _fechaInicioContrato; }
            set { _fechaInicioContrato = value; }
        }
        private int _idRegion;
        public int IdRegion
        {
            get { return _idRegion; }
            set { _idRegion = value; }
        }
        private int _idComuna;
        public int IdComuna
        {
            get { return _idComuna; }
            set { _idComuna = value; }
        }
        private string _direccion;
        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }
        private string _numero;
        public string Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }
        private string _restoDireccion;
        public string RestoDireccion
        {
            get { return _restoDireccion; }
            set { _restoDireccion = value; }
        }
        private int _cantidadAviso;
        public int CantidadAviso
        {
            get { return _cantidadAviso; }
            set { _cantidadAviso = value; }
        }
        #endregion
    }
}
