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
        public HttpResponseMessage Get([FromUri]string rol, [FromUri]string ecolId)
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
            else
            {
                int ecolIdInt = int.Parse(ecolId);
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
                    userlist = userlist.FindAll(p => p.EncoId == ecolIdInt);
                }
                httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, userlist);

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
    }
}