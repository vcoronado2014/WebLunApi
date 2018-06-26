using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.Profile;
using System.Runtime.Caching;

namespace RayenSalud.WebLun.Negocio.Usuarios
{
    public class Usuarios
    {
        public static ObjectCache cacheUs = MemoryCache.Default;
        public static DateTimeOffset tiempoCacheUs = Cache.Medio();
        public static List<Entidad.Usuarios> ObtenerUsuarios(string rol, string ecolId, string usuario)
        {
            List<Entidad.Usuarios> listaCache = (List<Entidad.Usuarios>)cacheUs.Get("ListarUsuarios");
            List<Entidad.Usuarios> userlist = new List<Entidad.Usuarios>();

            if (listaCache == null)
            {

                int ecolIdInt = int.Parse(ecolId);
                string rolStr = rol;
                bool esSuper = false;
                if (rolStr == "Super Administrador")
                {
                    esSuper = true;
                }

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
                        Entidad.Usuarios us = new Entidad.Usuarios();
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

                //caché  ****************************************************************
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = tiempoCacheUs;
                cacheUs.Add("ListarUsuarios", userlist, policy);
                //***********************************************************************

            }
            else
            {
                userlist = listaCache;
            }

            return userlist;

        }

        public static Entidad.Usuarios CrearUsuario(string nombreUsuario, string [] roles, string ecolId, string apellidoPaterno, string apellidoMaterno, string direccion, string idRegion, string idComuna, string nombres,
            string rut, string telefonoCelular, string telefonoFijo, string estamento, string restoDireccion, bool veReportes, string nombreContratante, string usuarioCreador, string email, string password, string pregunta, string respuesta)
        {
            Entidad.Usuarios usRe = new Entidad.Usuarios();

            try
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
                usRe.Rut = rut;
                usRe.TelefonoCelular = telefonoCelular;
                usRe.TelefonoFijo = telefonoFijo;
                #endregion

                //manejo del caching
                List<Entidad.Usuarios> listaCache = (List<Entidad.Usuarios>)cacheUs.Get("ListarUsuarios");
                if (listaCache != null)
                {
                    if (!listaCache.Exists(p => p.NombreUsuario == nombreUsuario))
                    {
                        listaCache.Add(usRe);
                    }

                }

            }
            catch(Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
            }

            return usRe;

        }
        public static Entidad.Usuarios ModificarUsuario(MembershipUser us, string ecolId, string apellidoPaterno, string apellidoMaterno, string direccion, string idRegion,
            string idComuna, string nombres, string rut, string telefonoCelular, string telefonoFijo, string estamento, string restoDireccion, bool veReportes, string nombreContratante,
            bool modificacionClave, string nombreUsuario, string password, string pregunta, string respuesta, string [] roles, string email, string usuarioCreador)
        {

            Entidad.Usuarios usRe = new Entidad.Usuarios();

            try
            {
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
                usRe.Rut = rut;
                usRe.TelefonoCelular = telefonoCelular;
                usRe.TelefonoFijo = telefonoFijo;
                #endregion

                //manejo del caching
                List<Entidad.Usuarios> listaCache = (List<Entidad.Usuarios>)cacheUs.Get("ListarUsuarios");
                if (listaCache != null)
                {
                    if (listaCache.Exists(p => p.NombreUsuario == nombreUsuario))
                    {
                        //si existe se modifica
                        Entidad.Usuarios usModificar = listaCache.Find(p => p.NombreUsuario == nombreUsuario);
                        if (usModificar != null)
                        {
                            //usModificar = usRe;
                            listaCache.Remove(usModificar);
                            listaCache.Add(usRe);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
            }
            return usRe;

        }

        public static bool EliminarUsuario(string usuarioEliminar)
        {
            bool retorno = false;
            try
            {
                MembershipUser us = Membership.GetUser(usuarioEliminar);


                retorno = Membership.DeleteUser(usuarioEliminar, true);
                if (retorno)
                {
                    //lo quitamos de la lista si es que no está
                    List<Entidad.Usuarios> listaCache = (List<Entidad.Usuarios>)cacheUs.Get("ListarUsuarios");
                    if (listaCache != null)
                    {
                        if (listaCache.Exists(p => p.NombreUsuario == usuarioEliminar))
                        {
                            //si existe se modifica
                            Entidad.Usuarios usModificar = listaCache.Find(p => p.NombreUsuario == usuarioEliminar);
                            if (usModificar != null)
                            {
                                listaCache.Remove(usModificar);
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Negocio.Utiles.NLogs(ex);
            }

            return retorno;
        }


    }


    public class Cache
    {
        /// <summary>
        /// 1 hora
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset Medio()
        {
            return DateTimeOffset.Now.AddHours(1);
        }
        /// <summary>
        /// 12 horas
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset ExtraFuerte()
        {
            return DateTimeOffset.Now.AddHours(12);
        }
        /// <summary>
        /// 60 segundos
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset Volatil()
        {
            return DateTimeOffset.Now.AddMinutes(1);
        }
    }
}
