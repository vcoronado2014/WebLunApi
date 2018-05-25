using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Global
{
    public class RebalseLun : EntidadBase
    {
        private int _encoId;
        public int EncoId
        {
            get { return _encoId; }
            set { _encoId = value; }
        }
        private int _totalLicencias;
        public int TotalLicencias
        {
            get { return _totalLicencias; }
            set { _totalLicencias = value; }
        }
        private int _sobrecupo;
        public int Sobrecupo
        {
            get { return _sobrecupo; }
            set { _sobrecupo = value; }
        }
        private int _totalLicenciasInicial;
        public int TotalLicenciasInicial
        {
            get { return _totalLicenciasInicial; }
            set { _totalLicenciasInicial = value; }
        }
        private int _totalSobrecupoInicial;
        public int TotalSobrecupoInicial
        {
            get { return _totalSobrecupoInicial; }
            set { _totalSobrecupoInicial = value; }
        }
    }

    public class EnvoltorioRebalseContratante : RebalseLun
    {
        private string _nombre;
        public string NombreEntidad
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public string UsuarioCreador { get; set; }
        public string Mensaje { get; set; }
    }
}
