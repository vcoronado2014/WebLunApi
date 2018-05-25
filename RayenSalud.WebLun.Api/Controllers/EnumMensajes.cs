using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RayenSalud.WebLun.Api.Controllers
{
    public enum EnumMensajes
    {
        Correcto = 0,
        Excepcion = 1000,
        Usuario_no_existe = 1,
        Clave_incorrecta = 2,
        Sin_persona_asociada = 3,
        Sin_Rol_asociado = 4,
        Inactivo_o_Eliminado = 5,
        Parametro_vacio_o_invalido = 6,
        Usuario_ya_existe = 7,
        Registro_creado_con_exito = 8,
        Registro_modificado_con_exito = 9,
        Registro_desactivado_con_exito = 10,
        Registro_eliminado_con_exito = 11,
        Excede_maximo_permitido_de_usuarios = 12
    }
}