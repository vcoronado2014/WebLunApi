using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using System.Web.Profile;
using RayenSalud.WebLun.Entidad;

namespace RayenSalud.WebLun.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController: ApiController
    {
        [AcceptVerbs("OPTIONS")]
        public void Options()
        { }

        [System.Web.Http.AcceptVerbs("POST")]
        public HttpResponseMessage Post(dynamic DynamicClass)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            if (data.Usuario == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Usuario");
            }
            else if (data.Clave == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Clave");
            }
            else
            {
                try
                {
                    string usuario = data.Usuario;
                    string clave = data.Clave;
                    Entidad.Usuarios us = new Entidad.Usuarios();

                    bool correcto = Membership.ValidateUser(usuario, clave);
                    if (correcto)
                    {
                        //obtenemos al usuario
                        MembershipUser user = Membership.GetUser(usuario);
                        if (user != null)
                        {
                            //datos propios 
                            us.Emmail = user.Email;
                            us.NombreUsuario = user.UserName;
                            us.Aprobado = user.IsApproved;
                            //******************************************
                            ProfileBase prof = ProfileBase.Create(usuario);
                            string[] roles =  Roles.GetRolesForUser(usuario);
                            if (prof != null)
                            {
                                //asociar los elementos necesarios
                                us.EncoId = (int)prof.GetPropertyValue("EncoId");
                                if (us.EncoId < 1)
                                {
                                    if (roles.Length > 0)
                                    {
                                        if (roles[0] == "Super Administrador")
                                        {
                                            us.EncoId = 1; //por defecto es rayen salud
                                        }
                                    }
                                }
                                us.Rut = (string)prof.GetPropertyValue("Rut");
                                us.Nombres = (string)prof.GetPropertyValue("Nombres");
                                us.ApellidoPaterno = (string)prof.GetPropertyValue("ApellidoPaterno");
                                us.ApellidoMaterno = (string)prof.GetPropertyValue("ApellidoMaterno");
                                us.Direccion = (string)prof.GetPropertyValue("Direccion");
                                us.RestoDireccion = (string)prof.GetPropertyValue("RestoDireccion");
                                us.TelefonoFijo = (string)prof.GetPropertyValue("TelefonoFijo");
                                us.TelefonoCelular = (string)prof.GetPropertyValue("TelefonoCelular");
                                us.Estamento = (string)prof.GetPropertyValue("Estamento");
                                us.Contratante = (string)prof.GetPropertyValue("Contratante");
                                us.IdRegion = (int)prof.GetPropertyValue("IdRegion");
                                us.IdComuna = (int)prof.GetPropertyValue("IdComuna");
                                us.VeReportes = (bool)prof.GetPropertyValue("VeReportes");
                                us.NombreCompleto = us.Nombres + " " + us.ApellidoPaterno + " " + us.ApellidoMaterno;
                                us.TokenSession = Negocio.Utiles.CrearTokenSession(us.EncoId, us.NombreUsuario);

                            }
                            if (us.VeReportes)
                                us.VeReportesTexto = "Si";
                            else
                                us.VeReportesTexto = "No";
                            //obtención de la region
                            RayenSalud.WebLun.Entidad.Territorio.Region region = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerRegionPorId(us.IdRegion);
                            if (region != null)
                            {
                                us.NombreRegion = region.Nombre;
                            }
                            RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerComunaPorId(us.IdComuna);
                            if (comuna != null)
                            {
                                us.NombreComuna = comuna.Nombre;
                            }
                            RayenSalud.WebLun.Entidad.Global.ContratanteLun contratante = new RayenSalud.WebLun.Entidad.Global.ContratanteLun();
                            //obtenemos el contratante por el nombre
                            if (us.EncoId > 0)
                            {
                                contratante = RayenSalud.WebLun.Negocio.Global.Global.ObtenerContratanteLunPorId(us.EncoId);
                            }

                            if (contratante != null)
                                us.Contratante = contratante.RazonSocial;

                            us.RolesUsuarios = roles;
                            if (roles != null)
                            {
                                if (roles.Length > 0)
                                {
                                    us.RolUsuario = roles[0];
                                }
                            }
                            //insertemos a la tabla login el usuario recien logueado
                            bool registroInsertado = Negocio.Global.Global.InsertarLGU(us.NombreUsuario, us.EncoId, us.RolUsuario, us.TokenSession);
                            //bool validar =  Negocio.Utiles.ValidarTokenSession(us.EncoId, us.NombreUsuario, us.TokenSession);

                            httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, us);
                        }
                        else
                        {
                            httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Usuario_no_existe);
                        }
                    }
                    else
                    {
                        httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Clave_incorrecta);
                    }
                    
                }
                catch (Exception ex)
                {
                    Negocio.Utiles.NLogs(ex);
                    httpResponse = ManejoMensajes.RetornaMensajeExcepcion(httpResponse, ex);
                }
            }
            return httpResponse;
        }
        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Get([FromUri]string rol, [FromUri]string ecolId, [FromUri]string token, [FromUri]string usuario)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            if (rol == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Rol Id");
            }
            else if (ecolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Ecol Id");
            }
            else if (token == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Token");
            }
            else if (usuario == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Usuario");
            }
            else
            {

                int ecolIdInt = int.Parse(ecolId);
                string rolStr = rol;
                bool esSuper = false;
                if (rolStr == "Super Administrador")
                {
                    esSuper = true;
                }
                string tokenSession = token;
                //ahora validamos el token
                bool esValido = Negocio.Utiles.ValidarTokenSession(ecolIdInt, usuario, tokenSession);

                if (esValido)
                {
                    //ahora nos traemos a los usuarios de la entidad contratante o bien a todo si el rol es Super Administrador
                    List<Usuarios> userlist = new List<Usuarios>();
                    MembershipUserCollection listadeUsuarios = Membership.GetAllUsers();
                    foreach (MembershipUser user in listadeUsuarios)
                    {
                        //solo los de la app, ya que existen usuarios de vio salud aca
                        string[] rolesEvaluar = Roles.GetRolesForUser(user.UserName);
                        if (Roles.IsUserInRole(user.UserName, "Administrador Lun") ||
                        Roles.IsUserInRole(user.UserName, "Administrador Web") ||
                        Roles.IsUserInRole(user.UserName, "Consultador Lun") ||
                        Roles.IsUserInRole(user.UserName, "Super Administrador"))
                        {
                            Usuarios us = new Usuarios();
                            string[] roles = Roles.GetRolesForUser(user.UserName);

                            us.Emmail = user.Email;
                            us.NombreUsuario = user.UserName;
                            us.Aprobado = user.IsApproved;
                            //******************************************
                            ProfileBase prof = ProfileBase.Create(user.UserName);

                            if (prof != null)
                            {
                                //asociar los elementos necesarios
                                us.EncoId = (int)prof.GetPropertyValue("EncoId");
                                us.Rut = (string)prof.GetPropertyValue("Rut");
                                us.Nombres = (string)prof.GetPropertyValue("Nombres");
                                us.ApellidoPaterno = (string)prof.GetPropertyValue("ApellidoPaterno");
                                us.ApellidoMaterno = (string)prof.GetPropertyValue("ApellidoMaterno");
                                us.Direccion = (string)prof.GetPropertyValue("Direccion");
                                us.RestoDireccion = (string)prof.GetPropertyValue("RestoDireccion");
                                us.TelefonoFijo = (string)prof.GetPropertyValue("TelefonoFijo");
                                us.TelefonoCelular = (string)prof.GetPropertyValue("TelefonoCelular");
                                us.Estamento = (string)prof.GetPropertyValue("Estamento");
                                us.Contratante = (string)prof.GetPropertyValue("Contratante");
                                us.IdRegion = (int)prof.GetPropertyValue("IdRegion");
                                us.IdComuna = (int)prof.GetPropertyValue("IdComuna");
                                us.VeReportes = (bool)prof.GetPropertyValue("VeReportes");
                                us.NombreCompleto = us.Nombres + " " + us.ApellidoPaterno + " " + us.ApellidoMaterno;

                            }
                            if (us.VeReportes)
                                us.VeReportesTexto = "Si";
                            else
                                us.VeReportesTexto = "No";
                            //obtención de la region
                            RayenSalud.WebLun.Entidad.Territorio.Region region = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerRegionPorId(us.IdRegion);
                            if (region != null)
                            {
                                us.NombreRegion = region.Nombre;
                            }
                            RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerComunaPorId(us.IdComuna);
                            if (comuna != null)
                            {
                                us.NombreComuna = comuna.Nombre;
                            }
                            RayenSalud.WebLun.Entidad.Global.ContratanteLun contratante = new RayenSalud.WebLun.Entidad.Global.ContratanteLun();
                            //obtenemos el contratante por el nombre
                            if (us.EncoId > 0)
                            {
                                contratante = RayenSalud.WebLun.Negocio.Global.Global.ObtenerContratanteLunPorId(us.EncoId);
                            }

                            if (contratante != null)
                                us.Contratante = contratante.RazonSocial;

                            us.RolesUsuarios = roles;
                            if (roles != null)
                            {
                                if (roles.Length > 0)
                                {
                                    us.RolUsuario = roles[0];
                                }
                            }

                            userlist.Add(us);

                        }

                    }
                    //ACA EVALUAMOS
                    if (userlist != null && userlist.Count > 0)
                    {
                        if (!esSuper)
                        {
                            userlist = userlist.FindAll(p => p.RolUsuario != "Super Administrador");
                        }
                        userlist = userlist.FindAll(p => p.EncoId == ecolIdInt);
                    }
                    httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, userlist);
                }
                else
                {
                    httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Token_invalido);
                }



            }

            return httpResponse;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Get([FromUri]string rol)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            if (rol == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Rol Id");
            }
            else
            {
                string rolStr = rol;
                bool esSuper = false;
                if (rolStr == "Super Administrador")
                {
                    esSuper = true;
                }
                
                //ahora nos traemos a los usuarios de la entidad contratante o bien a todo si el rol es Super Administrador
                List<Usuarios> userlist = new List<Usuarios>();
                MembershipUserCollection listadeUsuarios = Membership.GetAllUsers();
                foreach (MembershipUser user in listadeUsuarios)
                {
                    //solo los de la app, ya que existen usuarios de vio salud aca
                    string[] rolesEvaluar = Roles.GetRolesForUser(user.UserName);
                    if (Roles.IsUserInRole(user.UserName, "Administrador Lun") ||
                    Roles.IsUserInRole(user.UserName, "Administrador Web") ||
                    Roles.IsUserInRole(user.UserName, "Consultador Lun") ||
                    Roles.IsUserInRole(user.UserName, "Super Administrador"))
                    {
                        Usuarios us = new Usuarios();
                        string[] roles = Roles.GetRolesForUser(user.UserName);

                        us.Emmail = user.Email;
                        us.NombreUsuario = user.UserName;
                        us.Aprobado = user.IsApproved;
                        //******************************************
                        ProfileBase prof = ProfileBase.Create(user.UserName);

                        if (prof != null)
                        {
                            //asociar los elementos necesarios
                            us.EncoId = (int)prof.GetPropertyValue("EncoId");
                            us.Rut = (string)prof.GetPropertyValue("Rut");
                            us.Nombres = (string)prof.GetPropertyValue("Nombres");
                            us.ApellidoPaterno = (string)prof.GetPropertyValue("ApellidoPaterno");
                            us.ApellidoMaterno = (string)prof.GetPropertyValue("ApellidoMaterno");
                            us.Direccion = (string)prof.GetPropertyValue("Direccion");
                            us.RestoDireccion = (string)prof.GetPropertyValue("RestoDireccion");
                            us.TelefonoFijo = (string)prof.GetPropertyValue("TelefonoFijo");
                            us.TelefonoCelular = (string)prof.GetPropertyValue("TelefonoCelular");
                            us.Estamento = (string)prof.GetPropertyValue("Estamento");
                            us.Contratante = (string)prof.GetPropertyValue("Contratante");
                            us.IdRegion = (int)prof.GetPropertyValue("IdRegion");
                            us.IdComuna = (int)prof.GetPropertyValue("IdComuna");
                            us.VeReportes = (bool)prof.GetPropertyValue("VeReportes");
                            us.NombreCompleto = us.Nombres + " " + us.ApellidoPaterno + " " + us.ApellidoMaterno;

                        }
                        if (us.VeReportes)
                            us.VeReportesTexto = "Si";
                        else
                            us.VeReportesTexto = "No";
                        //obtención de la region
                        RayenSalud.WebLun.Entidad.Territorio.Region region = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerRegionPorId(us.IdRegion);
                        if (region != null)
                        {
                            us.NombreRegion = region.Nombre;
                        }
                        RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerComunaPorId(us.IdComuna);
                        if (comuna != null)
                        {
                            us.NombreComuna = comuna.Nombre;
                        }
                        RayenSalud.WebLun.Entidad.Global.ContratanteLun contratante = new RayenSalud.WebLun.Entidad.Global.ContratanteLun();
                        //obtenemos el contratante por el nombre
                        if (us.EncoId > 0)
                        {
                            contratante = RayenSalud.WebLun.Negocio.Global.Global.ObtenerContratanteLunPorId(us.EncoId);
                        }

                        if (contratante != null)
                            us.Contratante = contratante.RazonSocial;

                        us.RolesUsuarios = roles;
                        if (roles != null)
                        {
                            if (roles.Length > 0)
                            {
                                us.RolUsuario = roles[0];
                            }
                        }

                        userlist.Add(us);

                    }

                }
                //ACA EVALUAMOS
                if (userlist != null && userlist.Count > 0)
                {
                    if (!esSuper)
                    {
                        userlist = userlist.FindAll(p => p.RolUsuario != "Super Administrador");
                    }
                }
                httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, userlist);

            }

            return httpResponse;
        }

        [System.Web.Http.AcceptVerbs("DELETE")]
        public HttpResponseMessage Delete([FromUri]string usuarioEliminar, [FromUri]string usuarioEliminador, [FromUri]string token, [FromUri]string ecolId)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            if (usuarioEliminador == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Usuario Eliminador");
            }
            else if (usuarioEliminar == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Usuario Eliminar");
            }
            else if (token == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Token");
            }
            else if (ecolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Ecol Id");
            }
            else
            {

                int ecolIdInt = int.Parse(ecolId);

                string tokenSession = token;
                //ahora validamos el token
                bool esValido = Negocio.Utiles.ValidarTokenSession(ecolIdInt, usuarioEliminador, tokenSession);

                if (esValido)
                {

                    bool eliminado = false;
                    if ((Membership.FindUsersByName(usuarioEliminar).Count == 0))
                    {
                        httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Usuario_no_existe);
                    }
                    else
                    {

                        MembershipUser us = Membership.GetUser(usuarioEliminar);


                        eliminado = Membership.DeleteUser(usuarioEliminar, true);
                        //inserción registro en el historial
                        RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun historial = new Entidad.Global.HistorialEncargadoLun();
                        historial.TipoMovimiento = 4;//eliminar
                        historial.FechaRegistro = DateTime.Now;
                        historial.RunEncargado = "";
                        historial.UsuarioCreador = usuarioEliminador;
                        historial.UserEncargado = usuarioEliminar;
                        RayenSalud.WebLun.Negocio.Global.Global.InsertarRegistroHistorialLun(historial);



                    }

                    //return retorno;

                    httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, eliminado);
                }
                else
                {
                    httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Token_invalido);
                }



            }

            return httpResponse;
        }

        [System.Web.Http.AcceptVerbs("PUT")]
        public HttpResponseMessage Put(
            dynamic DynamicClass
            )
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            bool aprobado = true;
            string pregunta = "Hola";
            string respuesta = "que tal";

            if (data.EsNuevo == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Es Nuevo");
            }
            else if (data.NombreUsuario == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Nombre Usuario");
            }
            else if (data.Email == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Email");
            }
            else if (data.Rol == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Rol");
            }
            else if (data.EcolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Ecol ID");
            }
            else if (data.ApellidoPaterno == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Apellido Paterno");
            }
            else if (data.Direccion == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Direccion");
            }
            else if (data.IdRegion == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Id Region");
            }
            else if (data.IdComuna == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Id Comuna");
            }
            else if (data.Nombres == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Nombres");
            }
            else if (data.Rut == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Rut");
            }
            else if (data.Estamento == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Estamento");
            }
            else if (data.NombreContratante == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Nombre Contratante");
            }
            else if (data.UsuarioCreador == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Usuario Creador");
            }
            else
            {
                string esNuevo = data.EsNuevo;
                bool esNuevoB = Convert.ToBoolean(esNuevo);
                string nombreUsuario = data.NombreUsuario;
                string email = data.Email;
                string rol = data.Rol;
                string ecolId = data.EcolId;
                string apellidoPaterno = data.ApellidoPaterno;
                string direccion = data.Direccion;
                string idRegion = data.IdRegion;
                string idComuna = data.IdComuna;
                string nombres = data.Nombres;
                string rut = data.Rut;
                string estamento = data.Estamento;
                string nombreContratante = data.NombreContratante;
                string usuarioCreador = data.UsuarioCreador;
                //no requerido
                string apellidoMaterno = "";
                string telefonoCelular = "";
                string telefonoFijo = "";
                string restoDireccion = "";
                string veReportesStr = "false";
                bool veReportes = false;
                if (data.ApellidoMaterno != null)
                {
                    apellidoMaterno = data.ApellidoMaterno;
                }
                if (data.TelefonoFijo != null)
                {
                    telefonoFijo = data.TelefonoFijo;
                }
                if (data.TelefonoCelular != null)
                {
                    telefonoCelular = data.TelefonoCelular;
                }
                if (data.RestoDireccion != null)
                {
                    restoDireccion = data.RestoDireccion;
                }
                if (data.VeReportes != null)
                {
                    veReportesStr = data.VeReportes;
                    veReportes = Convert.ToBoolean(veReportesStr);
                }
                //ahora procesamos ese rol
                string[] roles = new string[] { rol.ToString() };

                string password = "";
                if (data.Password != null)
                {
                    password = data.Password;
                }
                #region es nuevo
                if (esNuevoB)
                {
                    //verificamos si ya existe el nombre de usuario o no
                    if ((Membership.FindUsersByName(nombreUsuario).Count == 0))
                    {
                        //primero lo creamos

                        MembershipCreateStatus status;
                        Membership.CreateUser(nombreUsuario, password, email, pregunta, respuesta, aprobado, out status);
                        if (status == MembershipCreateStatus.Success)
                        {
                            #region creacion de usuario
                            //todo ok, seguimos
                            //lo volvemos a recuperar
                            MembershipUser mu = Membership.GetUser(nombreUsuario);
                            //agregamos los roles al usuario
                            Roles.AddUserToRoles(mu.UserName, roles);
                            //actualizamos el usuario
                            Membership.UpdateUser(mu);
                            //ahora los perfiles
                            ProfileBase prof = ProfileBase.Create(mu.UserName);
                            prof.SetPropertyValue("EncoId", int.Parse(ecolId));
                            prof.SetPropertyValue("ApellidoPaterno", apellidoPaterno);
                            prof.SetPropertyValue("ApellidoMaterno", apellidoMaterno);
                            prof.SetPropertyValue("Direccion", direccion);
                            prof.SetPropertyValue("IdRegion", int.Parse(idRegion));
                            prof.SetPropertyValue("IdComuna", int.Parse(idComuna));
                            prof.SetPropertyValue("Nombres", nombres);
                            prof.SetPropertyValue("Rut", rut);
                            prof.SetPropertyValue("TelefonoCelular", telefonoCelular);
                            prof.SetPropertyValue("TelefonoFijo", telefonoFijo);
                            prof.SetPropertyValue("Estamento", estamento);
                            prof.SetPropertyValue("RestoDireccion", restoDireccion);
                            prof.SetPropertyValue("VeReportes", veReportes);
                            //para obtener la entidad contratante
                            prof.SetPropertyValue("Contratante", nombreContratante);
                            //ahora guardamos el perfil
                            prof.Save();
                            //ahora guardamos al historial
                            RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun historial =
                                    new RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun();
                            historial.TipoMovimiento = 1;//crear
                            historial.FechaRegistro = DateTime.Now;
                            historial.RunEncargado = rut;
                            historial.UsuarioCreador = usuarioCreador;
                            historial.UserEncargado = nombreUsuario;
                            RayenSalud.WebLun.Negocio.Global.Global.InsertarRegistroHistorialLun(historial);
                            #endregion
                            #region nueva entidad a devolver
                            Entidad.Usuarios usRe = new Entidad.Usuarios();
                            usRe.UsuarioCreador = usuarioCreador;
                            usRe.VeReportes = veReportes;
                            if (veReportes)
                                usRe.VeReportesTexto = "Si";
                            else
                                usRe.VeReportesTexto = "No";

                            usRe.ApellidoMaterno = apellidoMaterno;
                            usRe.ApellidoPaterno = apellidoPaterno;
                            usRe.Aprobado = true;
                            usRe.Contratante = nombreContratante;
                            usRe.Direccion = direccion;
                            usRe.Emmail = email;
                            usRe.EncoId = int.Parse(ecolId);
                            usRe.Estamento = estamento;
                            usRe.IdComuna = int.Parse(idComuna);
                            usRe.IdRegion = int.Parse(idRegion);
                            usRe.NombreCompleto = nombres + " " + apellidoPaterno + " " + apellidoMaterno;
                            //usRe.NombreComuna = 
                            RayenSalud.WebLun.Entidad.Territorio.Region region = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerRegionPorId(int.Parse(idRegion));
                            if (region != null)
                            {
                                usRe.NombreRegion = region.Nombre;
                            }
                            RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerComunaPorId(int.Parse(idComuna));
                            if (comuna != null)
                            {
                                usRe.NombreComuna = comuna.Nombre;
                            }
                            usRe.Nombres = nombres;
                            usRe.NombreUsuario = nombreUsuario;
                            usRe.Password = password;
                            usRe.Pregunta = pregunta;
                            usRe.Respuesta = respuesta;
                            usRe.RestoDireccion = restoDireccion;
                            usRe.RolesUsuarios = roles;
                            usRe.RolUsuario = roles[0].ToString();
                            #endregion
                            //todo correcto, retornamos la data
                            httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, usRe, EnumMensajes.Registro_creado_con_exito);

                        }
                        else
                        {
                            httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, 3000, status.ToString());
                        }
                    }
                    else
                    {
                        //esta creando un usuario con un nombre de usuario que ya existe, no se puede
                        httpResponse = ManejoMensajes.RetornaMensajeError(httpResponse, EnumMensajes.Usuario_ya_existe);
                    }

                }
                #endregion
                #region antiguo

                else
                {
                    //aca debemos controlar también el cambio de clave
                    bool modificacionClave = false;
                    MembershipUser us = Membership.GetUser(nombreUsuario);
                    if (!string.IsNullOrEmpty(password))
                    {
                        Membership.DeleteUser(us.UserName, true);
                        MembershipCreateStatus status;
                        Membership.CreateUser(nombreUsuario, password, email, "hola", "que tal", true, out status);
                        if (status != MembershipCreateStatus.Success)
                        {
                            throw new ApplicationException(status.ToString());
                        }
                        modificacionClave = true;
                    }
                    //ahora seteamos los nuevo valores
                    us.Email = email;
                    us.IsApproved = true;
                    //manejo de roles
                    if (roles != null && roles.Length > 0)
                    {

                        for (int r = 0; r < Roles.GetAllRoles().Length; r++)
                        {

                                if (Roles.IsUserInRole(us.UserName, Roles.GetAllRoles()[r].ToString()))
                                    Roles.RemoveUserFromRole(us.UserName.ToString(), Roles.GetAllRoles()[r].ToString());
 
                        }

                        for (int t = 0; t < roles.Length; t++)
                        {
                            if (roles[t].ToString() != "")
                            {
                                if (!Roles.IsUserInRole(us.UserName, roles[t].ToString()))
                                    Roles.AddUserToRole(us.UserName.ToString(), roles[t].ToString());
                            }
                        }
                    }
                    //actualizamos al usuario
                    Membership.UpdateUser(us);
                    //ahora los perfiles
                    ProfileBase prof = ProfileBase.Create(us.UserName);
                    prof.SetPropertyValue("EncoId", int.Parse(ecolId));
                    prof.SetPropertyValue("ApellidoPaterno", apellidoPaterno);
                    prof.SetPropertyValue("ApellidoMaterno", apellidoMaterno);
                    prof.SetPropertyValue("Direccion", direccion);
                    prof.SetPropertyValue("IdRegion", int.Parse(idRegion));
                    prof.SetPropertyValue("IdComuna", int.Parse(idComuna));
                    prof.SetPropertyValue("Nombres", nombres);
                    prof.SetPropertyValue("Rut", rut);
                    prof.SetPropertyValue("TelefonoCelular", telefonoCelular);
                    prof.SetPropertyValue("TelefonoFijo", telefonoFijo);
                    prof.SetPropertyValue("Estamento", estamento);
                    prof.SetPropertyValue("RestoDireccion", restoDireccion);
                    prof.SetPropertyValue("VeReportes", veReportes);
                    //para obtener la entidad contratante
                    prof.SetPropertyValue("Contratante", nombreContratante);
                    //ahora guardamos el perfil
                    prof.Save();
                    //ahora historial
                    RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun historial =
                        new RayenSalud.WebLun.Entidad.Global.HistorialEncargadoLun();
                    if (modificacionClave)
                        historial.TipoMovimiento = 3;//modificacion clave
                    else
                        historial.TipoMovimiento = 2;//modificacion usuario
                    historial.FechaRegistro = DateTime.Now;
                    historial.RunEncargado = rut;
                    historial.UsuarioCreador = usuarioCreador;
                    historial.UserEncargado = nombreUsuario;
                    RayenSalud.WebLun.Negocio.Global.Global.InsertarRegistroHistorialLun(historial);
                    //hasta aca tgodo bien, por lo tanto hay que devolver un usuario con todos los elementos modificados
                    #region nueva entidad a devolver
                    Entidad.Usuarios usRe = new Entidad.Usuarios();
                    usRe.UsuarioCreador = usuarioCreador;
                    usRe.VeReportes = veReportes;
                    if (veReportes)
                        usRe.VeReportesTexto = "Si";
                    else
                        usRe.VeReportesTexto = "No";

                    usRe.ApellidoMaterno = apellidoMaterno;
                    usRe.ApellidoPaterno = apellidoPaterno;
                    usRe.Aprobado = true;
                    usRe.Contratante = nombreContratante;
                    usRe.Direccion = direccion;
                    usRe.Emmail = email;
                    usRe.EncoId = int.Parse(ecolId);
                    usRe.Estamento = estamento;
                    usRe.IdComuna = int.Parse(idComuna);
                    usRe.IdRegion = int.Parse(idRegion);
                    usRe.NombreCompleto = nombres + " " + apellidoPaterno + " " + apellidoMaterno;
                    //usRe.NombreComuna = 
                    RayenSalud.WebLun.Entidad.Territorio.Region region = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerRegionPorId(int.Parse(idRegion));
                    if (region != null)
                    {
                        usRe.NombreRegion = region.Nombre;
                    }
                    RayenSalud.WebLun.Entidad.Territorio.Comuna comuna = RayenSalud.WebLun.Negocio.Territorio.Territorio.ObtenerComunaPorId(int.Parse(idComuna));
                    if (comuna != null)
                    {
                        usRe.NombreComuna = comuna.Nombre;
                    }
                    usRe.Nombres = nombres;
                    usRe.NombreUsuario = nombreUsuario;
                    usRe.Password = password;
                    usRe.Pregunta = pregunta;
                    usRe.Respuesta = respuesta;
                    usRe.RestoDireccion = restoDireccion;
                    usRe.RolesUsuarios = roles;
                    usRe.RolUsuario = roles[0].ToString();
                    #endregion

                    if (modificacionClave)
                        httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, usRe, EnumMensajes.Clave_creada_con_exito);
                    else
                        httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, usRe, EnumMensajes.Registro_modificado_con_exito);
                }
                #endregion
  
            }
            //retornamos
            return httpResponse;
        }

    }
}