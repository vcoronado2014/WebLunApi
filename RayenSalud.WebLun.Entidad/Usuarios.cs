using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayenSalud.WebLun.Entidad
{
    public class Usuarios
    {
        #region Propiedades

        public string TokenSession { get; set; }
        public int _encoInd;
        public int EncoId
        {
            get
            {
                return _encoInd;
            }
            set
            {
                _encoInd = value;
            }
        }
        public string _rut;
        public string Rut
        {
            get
            {
                return _rut;
            }
            set
            {
                _rut = value;
            }
        }
        public string _nombres;
        public string Nombres
        {
            get
            {
                return _nombres;
            }
            set
            {
                _nombres = value;
            }
        }
        public string _apellidoPaterno;
        public string ApellidoPaterno
        {
            get
            {
                return _apellidoPaterno;
            }
            set
            {
                _apellidoPaterno = value;
            }
        }
        public string _apellidoMaterno;
        public string ApellidoMaterno
        {
            get
            {
                return _apellidoMaterno;
            }
            set
            {
                _apellidoMaterno = value;
            }
        }
        public string _direccion;
        public string Direccion
        {
            get
            {
                return _direccion;
            }
            set
            {
                _direccion = value;
            }
        }
        public string _restoDireccion;
        public string RestoDireccion
        {
            get
            {
                return _restoDireccion;
            }
            set
            {
                _restoDireccion = value;
            }
        }
        public string _telefonoFijo;
        public string TelefonoFijo
        {
            get
            {
                return _telefonoFijo;
            }
            set
            {
                _telefonoFijo = value;
            }
        }
        public string _telefonoCelular;
        public string TelefonoCelular
        {
            get
            {
                return _telefonoCelular;
            }
            set
            {
                _telefonoCelular = value;
            }
        }
        public int _idregion;
        public int IdRegion
        {
            get
            {
                return _idregion;
            }
            set
            {
                _idregion = value;
            }
        }
        public int _idComuna;
        public int IdComuna
        {
            get
            {
                return _idComuna;
            }
            set
            {
                _idComuna = value;
            }
        }
        public string _nombreUsuario;
        public string NombreUsuario
        {
            get
            {
                return _nombreUsuario;
            }
            set
            {
                _nombreUsuario = value;
            }
        }
        public string _email;
        public string Emmail
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        public string _pregunta;
        public string Pregunta
        {
            get
            {
                return _pregunta;
            }
            set
            {
                _pregunta = value;
            }
        }
        public string _respuesta;
        public string Respuesta
        {
            get
            {
                return _respuesta;
            }
            set
            {
                _respuesta = value;
            }
        }
        public string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        public bool _aprobado;
        public bool Aprobado
        {
            get
            {
                return _aprobado;
            }
            set
            {
                _aprobado = value;
            }
        }
        private string[] _rolesUsuario;
        public string[] RolesUsuarios
        {
            get
            {
                return _rolesUsuario;
            }
            set
            {
                _rolesUsuario = value;
            }
        }
        public string _nombreCompleto;
        public string NombreCompleto
        {
            get
            {
                return _nombreCompleto;
            }
            set
            {
                _nombreCompleto = value;
            }
        }
        public string _estamento;
        public string Estamento
        {
            get
            {
                return _estamento;
            }
            set
            {
                _estamento = value;
            }
        }
        public string _contratante;
        public string Contratante
        {
            get
            {
                return _contratante;
            }
            set
            {
                _contratante = value;
            }
        }
        public bool _veReportes;
        public bool VeReportes
        {
            get
            {
                return _veReportes;
            }
            set
            {
                _veReportes = value;
            }
        }
        public string _rolUsuario;
        public string RolUsuario
        {
            get
            {
                return _rolUsuario;
            }
            set
            {
                _rolUsuario = value;
            }
        }

        public string _nombreRegion;
        public string NombreRegion
        {
            get
            {
                return _nombreRegion;
            }
            set
            {
                _nombreRegion = value;
            }
        }
        public string _nombreComuna;
        public string NombreComuna
        {
            get
            {
                return _nombreComuna;
            }
            set
            {
                _nombreComuna = value;
            }
        }

        public string _usuarioCreador;
        public string UsuarioCreador
        {
            get
            {
                return _usuarioCreador;
            }
            set
            {
                _usuarioCreador = value;
            }
        }
        public string _veReportesTexto;
        public string VeReportesTexto
        {
            get
            {
                return _veReportesTexto;
            }
            set
            {
                _veReportesTexto = value;
            }
        }
        #endregion
    }
}
