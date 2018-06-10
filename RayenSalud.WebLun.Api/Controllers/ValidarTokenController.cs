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
    public class ValidarTokenController : ApiController
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

            if (data.EcolId == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "EcolId");
            }
            else if (data.NombreUsuario == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "NombreUsuario");
            }
            else if (data.Rol == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Rol");
            }
            else if (data.Token == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Token");
            }
            else if (data.Renovar == "")
            {
                httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Parametro_vacio_o_invalido, "Renovar");
            }
            else
            {
                try
                {
                    string ecolId = data.EcolId;
                    string nombreUsuario = data.NombreUsuario;
                    string token = data.Token;
                    string renovar = data.Renovar;
                    string rol = data.Rol;

                    bool renueva = false;
                    if (renovar == "TRUE" || renovar == "true" || renovar == "True")
                    {
                        renueva = true;
                    }

                    bool validar = false;
                    if (renueva)
                    {
                        string tokenNuevo = Negocio.Utiles.CrearTokenSession(int.Parse(ecolId), nombreUsuario);
                        Negocio.Global.Global.ModificarLGU(nombreUsuario, int.Parse(ecolId), rol, token, tokenNuevo);

                        httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, tokenNuevo);
                    }
                    else
                    {
                        validar = Negocio.Utiles.ValidarTokenSession(int.Parse(ecolId), nombreUsuario, token);
                        if (validar)
                        {
                            httpResponse = ManejoMensajes.RetornaMensajeCorrecto(httpResponse, token);
                        }
                        else
                        {
                            httpResponse = ManejoMensajes.RetornaMensajeParametroVacio(httpResponse, EnumMensajes.Token_invalido, "Token");
                        }
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
    }
}