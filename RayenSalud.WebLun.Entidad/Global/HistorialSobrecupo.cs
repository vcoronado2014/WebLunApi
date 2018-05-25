using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Global
{
    public class HistorialSobrecupo : EntidadBase
    {
        private int _encoId;
        public int EncoId
        {
            get { return _encoId; }
            set { _encoId = value; }
        }
        private DateTime _fecha;
        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
        private int _sobrecupo;
        public int Sobrecupo
        {
            get { return _sobrecupo; }
            set { _sobrecupo = value; }
        }
        private int _sobrecupoFinal;
        public int SobrecupoFinal
        {
            get { return _sobrecupoFinal; }
            set { _sobrecupoFinal = value; }
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
    }
}
