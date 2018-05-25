using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad.Lun
{
    public class RegistroLun : EntidadBase
    {
        public RegistroLun()
        {

        }
        private string _runFuncionario;
        public string RunFuncionario
        {
            get { return _runFuncionario; }
            set { _runFuncionario = value; }
        }
        private string _nombreCompleto;
        public string NombreCompleto
        {
            get { return _nombreCompleto; }
            set { _nombreCompleto = value; }
        }
        private string _nombreFuncionario;
        public string NombreFuncionario
        {
            get { return _nombreFuncionario; }
            set { _nombreFuncionario = value; }
        }
        private string _primerApellido;
        public string PrimerApellido
        {
            get { return _primerApellido; }
            set { _primerApellido = value; }
        }
        private string _segundoApellido;
        public string SegundoApellido
        {
            get { return _segundoApellido; }
            set { _segundoApellido = value; }
        }
        private int _sexoId;
        public int IdSexo
        {
            get { return _sexoId; }
            set { _sexoId = value; }
        }
        private int _encoId;
        public int EncoId
        {
            get { return _encoId; }
            set { _encoId = value; }
        }
        private DateTime _fechaAlta;
        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }
        private DateTime _fechaBaja;
        public DateTime FechaBaja
        {
            get { return _fechaBaja; }
            set { _fechaBaja = value; }
        }
        private DateTime _fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return _fechaCreacion; }
            set { _fechaCreacion = value; }
        }
        private int _esSobrecupo;
        public int EsSobrecupo
        {
            get { return _esSobrecupo; }
            set { _esSobrecupo = value; }
        }
        private string _usuarioCreador;
        public string UsuarioCreador
        {
            get { return _usuarioCreador; }
            set { _usuarioCreador = value; }
        }
        private int _esRayen;
        public int EsRayen
        {
            get { return _esRayen; }
            set { _esRayen = value; }
        }
        private int _esRayenComunitario;
        public int EsRayenComunitario
        {
            get { return _esRayenComunitario; }
            set { _esRayenComunitario = value; }
        }
        private int _esFlorence;
        public int EsFlorence
        {
            get { return _esFlorence; }
            set { _esFlorence = value; }
        }
        private DateTime _fechaModificacion;
        public DateTime FechaModificacion
        {
            get { return _fechaModificacion; }
            set { _fechaModificacion = value; }
        }
    }

    public class ListadoLunPertenece
    {
        public ListadoLunPertenece()
        {
        }
        public int IdNodo { get; set; }
        public string Establecimiento { get; set; }
        public bool Pertenece { get; set; }
        public string PerteneceString { get; set; }
    }
}
